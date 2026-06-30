# Script para descargar Monaco Editor localmente
# Ejecutar este script una vez para obtener Monaco Editor

$monacoVersion = "0.45.0"
$outputDir = "Casio880PEditor\wwwroot\monaco-editor"

Write-Host "Descargando Monaco Editor v$monacoVersion..." -ForegroundColor Green

# URL del paquete de Monaco Editor en npm
$url = "https://registry.npmjs.org/monaco-editor/-/monaco-editor-$monacoVersion.tgz"
$tempFile = "$env:TEMP\monaco-editor.tgz"

try {
    # Descargar el archivo tar
    Write-Host "Descargando desde $url..." -ForegroundColor Yellow
    Invoke-WebRequest -Uri $url -OutFile $tempFile -UseBasicParsing

    Write-Host "Extrayendo archivos..." -ForegroundColor Yellow

    # Crear directorio temporal
    $tempExtractDir = "$env:TEMP\monaco-extract"
    if (Test-Path $tempExtractDir) {
        Remove-Item $tempExtractDir -Recurse -Force
    }
    New-Item -ItemType Directory -Path $tempExtractDir | Out-Null

    # Extraer usando tar (disponible en Windows 10+)
    tar -xzf $tempFile -C $tempExtractDir

    # Copiar solo los archivos necesarios
    $sourceMin = Join-Path $tempExtractDir "package\min"
    $sourceDev = Join-Path $tempExtractDir "package\dev"

    if (Test-Path $sourceMin) {
        Write-Host "Copiando archivos min..." -ForegroundColor Yellow
        Copy-Item -Path "$sourceMin\*" -Destination $outputDir -Recurse -Force
    }

    # Limpiar
    Remove-Item $tempFile -Force -ErrorAction SilentlyContinue
    Remove-Item $tempExtractDir -Recurse -Force -ErrorAction SilentlyContinue

    Write-Host "¡Monaco Editor descargado exitosamente!" -ForegroundColor Green
    Write-Host "Archivos en: $outputDir" -ForegroundColor Cyan

} catch {
    Write-Host "Error: $_" -ForegroundColor Red
    Write-Host "Intentando método alternativo..." -ForegroundColor Yellow

    # Método alternativo: descargar desde unpkg.com
    $files = @(
        "vs/loader.js",
        "vs/editor/editor.main.js",
        "vs/editor/editor.main.css",
        "vs/editor/editor.main.nls.js",
        "vs/base/worker/workerMain.js"
    )

    foreach ($file in $files) {
        $fileUrl = "https://cdn.jsdelivr.net/npm/monaco-editor@$monacoVersion/min/$file"
        $outputFile = Join-Path $outputDir $file
        $outputFileDir = Split-Path $outputFile -Parent

        if (-not (Test-Path $outputFileDir)) {
            New-Item -ItemType Directory -Path $outputFileDir -Force | Out-Null
        }

        Write-Host "Descargando $file..." -ForegroundColor Yellow
        Invoke-WebRequest -Uri $fileUrl -OutFile $outputFile -UseBasicParsing
    }

    Write-Host "¡Archivos descargados con método alternativo!" -ForegroundColor Green
}
