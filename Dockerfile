# Imagem base para o runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Imagem para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar arquivos de projeto para resolver dependências
COPY *.sln ./
COPY Alpha-Manager-BackEnd/*.csproj Alpha-Manager-BackEnd/
COPY Alpha.Api/*.csproj Alpha.Api/
COPY Alpha.Application/*.csproj Alpha.Application/
COPY Alpha.Domain/*.csproj Alpha.Domain/
COPY Alpha.Persistences/*.csproj Alpha.Persistences/

# Restaurar dependências
RUN dotnet restore "Alpha-Manager-BackEnd/Alpha-Manager-BackEnd.csproj"

# Copiar todo o código
COPY . .

# Build da aplicação
RUN dotnet build "Alpha-Manager-BackEnd/Alpha-Manager-BackEnd.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "Alpha-Manager-BackEnd/Alpha-Manager-BackEnd.csproj" -c Release -o /app/publish

# Runtime final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Instalar dockerize (para aguardar o PostgreSQL)
RUN apt-get update && \
    apt-get install -y wget && \
    wget -O dockerize.tar.gz https://github.com/jwilder/dockerize/releases/download/v0.6.1/dockerize-linux-amd64-v0.6.1.tar.gz && \
    tar -xzvf dockerize.tar.gz -C /usr/local/bin && \
    rm dockerize.tar.gz

# Entrypoint com wait para PostgreSQL
ENTRYPOINT ["dockerize", "-wait", "tcp://postgres:5432", "-timeout", "120s", "--", "dotnet", "Alpha-Manager-BackEnd.dll"]