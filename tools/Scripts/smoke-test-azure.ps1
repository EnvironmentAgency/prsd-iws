# Enable -Verbose option
[CmdletBinding()]

param
(
    [Parameter(Mandatory=$true)]
    [uri]$Url = $null,
    
    [Parameter(Mandatory=$true)]
    [string]$TestEmail = $null,

    [Parameter(Mandatory=$true)]
    [string]$SiteUsername = $null,

    [Parameter(Mandatory=$true)]
    [string]$SitePassword = $null,

    [Parameter(Mandatory=$true)]
    [string]$VirusUrl = $null
)

$exitCode = 0;

Try
{
      $failedTests = @();

      [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
      ### Smoke test ###
      try 
      {
          $smokeTestUrl = New-Object System.Uri($Url, "admin/smoke-test");
          
          $smokeTestResult = Invoke-RestMethod $smokeTestUrl.ToString();
      }
      catch [System.Net.WebException]
      {
       
          # Suppress any exceptions
      }

      if ($smokeTestResult -ne "True")
      {
          $failedTests += "Failed admin/smoke-test";
      }

      ### Virus scan ###
      try 
      {
          $virusUrl = New-Object System.Uri($VirusUrl, "/api/scanner/scan"); 
          $virusResult = Invoke-RestMethod $virusUrl.ToString() -Method Post  -Body 'text' -ContentType 'application/x-www-form-urlencoded';
          
      }
      catch [System.Net.WebException]
      {
          if (!$virusResult.StatusCode -eq 403)
          { 
             $virusResult += "Failed virus check";
          }
      }      
      
      ### Login ###
      $loginUrl = New-Object System.Uri($Url, "account/login");
      $loginResult = Invoke-WebRequest $loginUrl.ToString() -SessionVariable session;
      
      $loginResult.Forms[0].Fields["Email"] = $SiteUsername;
      $loginResult.Forms[0].Fields["Password"] = $SitePassword;
      
      $loginPostResult = Invoke-WebRequest $loginUrl.ToString() -Method Post -Body $loginResult.Forms[0].Fields -ContentType 'application/x-www-form-urlencoded' -WebSession $session;

      if (!($loginPostResult.BaseResponse.ResponseUri -ne $loginUrl -and $loginPostResult.StatusCode -eq 200))
      { 
          $failedTests += "Failed login";
      }

       ### Send test email ###
      $testEmailUrl = New-Object System.Uri($Url, "admin/test-email");
      $testEmailSuccessUrl = New-Object System.Uri($Url, "admin/test-email/success");
      $testEmailResult = Invoke-WebRequest $testEmailUrl.ToString() -WebSession $session;
      
      if ($testEmailResult.BaseResponse.ResponseUri -ne $testEmailUrl)
      {
          $failedTests += "Access denied to send test email";
      }
      
      $testEmailResult.Forms[0].Fields["EmailTo"] = $TestEmail;
      
      $testEmailPostResult = Invoke-WebRequest $testEmailUrl.ToString() -Method Post -Body $testEmailResult.Forms[0].Fields -ContentType 'application/x-www-form-urlencoded' -WebSession $session;
      
      if ($testEmailPostResult.BaseResponse.ResponseUri -ne $testEmailSuccessUrl -and $testEmailPostResult.StatusCode -ne 200)
      {
          $failedTests += "Failed to send test email";
      }

      ### Send test file to scan ###
      $testScanUrl = New-Object System.Uri($Url, "admin/virus-scan");
      $testScanResult = Invoke-WebRequest $testScanUrl.ToString() -WebSession $session;
      
      if ($testScanResult.BaseResponse.ResponseUri -ne $testScanUrl)
      {
          $failedTests += "Access denied to scan file";
      }

      if ( $testEmailPostResult.StatusCode -ne 200)
      {
          $failedTests += "Scan file failed";
      }

      ### Report success or failure ###
      if ($failedTests.Length -gt 0)
      {
          $blockExitCode = -1;
          Write-Host "[FAILURE] : Smoke tests failed!";
      
          foreach ($failure in $failedTests)
          {
              Write-Host $failure;
          }
		$exitCode = -1;
      }
      else
      {
          Write-Host "[SUCCESS] : All smoke tests successful!";
      }
}
Catch
{      
   Write-Error -ErrorRecord $_;
   $exitCode = -1;
}

exit $exitCode;