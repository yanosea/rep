# build rep
dotnet publish src\rep\rep\rep.csproj -c Release -o rep\ -f net7.0-windows10.0.19041.0 -p WindowsPackageType=None

# compile rep-installer
iscc src\rep-installer\rep-installer.iss

# remove unnecessary directory
if (Test-Path -Path rep) {
  Remove-Item -Recurse -Force rep
}
