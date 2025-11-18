@echo off
echo ========================================
echo AuctoValue - First Time Setup
echo ========================================
echo.

REM Step 1: Copy configuration files
echo [1/6] Copying configuration files...
if not exist "Backend\appsettings.json" (
    copy "Backend\appsettings.json.example" "Backend\appsettings.json" >nul
    echo       Created Backend\appsettings.json
) else (
    echo       Backend\appsettings.json already exists, skipping...
)

if not exist "Frontend\.env" (
    copy "Frontend\.env.example" "Frontend\.env" >nul
    echo       Created Frontend\.env
) else (
    echo       Frontend\.env already exists, skipping...
)
echo.

REM Step 2: Restore and build backend
echo [2/6] Building backend...
cd Backend
dotnet restore >nul 2>&1
if %errorlevel% neq 0 (
    echo       ERROR: Failed to restore backend dependencies
    cd ..
    pause
    exit /b 1
)
dotnet build --configuration Release >nul 2>&1
if %errorlevel% neq 0 (
    echo       ERROR: Failed to build backend
    cd ..
    pause
    exit /b 1
)
echo       Backend built successfully
cd ..
echo.

REM Step 3: Install frontend dependencies
echo [3/6] Installing frontend dependencies...
cd Frontend
call npm install >nul 2>&1
if %errorlevel% neq 0 (
    echo       ERROR: Failed to install frontend dependencies
    cd ..
    pause
    exit /b 1
)
echo       Frontend dependencies installed
cd ..
echo.

REM Step 4: Run linter
echo [4/6] Running linter...
cd Frontend
call npm run lint >nul 2>&1
if %errorlevel% neq 0 (
    echo       WARNING: Linting issues detected (non-blocking)
) else (
    echo       Linting completed successfully
)
cd ..
echo.

REM Step 5: Run prettier
echo [5/6] Running code formatter...
cd Frontend
call npm run format >nul 2>&1
if %errorlevel% neq 0 (
    echo       WARNING: Formatting issues detected (non-blocking)
) else (
    echo       Code formatting completed
)
cd ..
echo.

REM Step 6: Run tests
echo [6/6] Running backend tests...
cd Tests
dotnet test --configuration Release >nul 2>&1
if %errorlevel% neq 0 (
    echo       WARNING: Some tests failed
) else (
    echo       All tests passed
)
cd ..
echo.

echo ========================================
echo Setup Complete!
echo ========================================
echo.
echo To start the application:
echo   Backend:  cd Backend  ^&^& dotnet run
echo   Frontend: cd Frontend ^&^& npm run dev
echo.
echo Or run start.bat to start both servers
echo ========================================
pause
