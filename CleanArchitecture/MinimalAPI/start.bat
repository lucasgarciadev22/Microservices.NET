@echo off
set IMAGE_NAME=lucasgarciadev22/tinybank:latest

REM Construir a imagem Docker da API
docker build -t %IMAGE_NAME% .

REM Fazer push da imagem para o Docker Hub
docker push %IMAGE_NAME%

REM Iniciar o Docker Compose
docker-compose up -d
