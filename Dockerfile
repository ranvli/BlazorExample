# Utilizar la imagen base del SDK de .NET 6.0
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar el archivo de solución
COPY FullApp.sln ./

# Copiar todos los archivos del proyecto
COPY FullApp/Server/. ./FullApp/Server/
COPY FullApp/Client/. ./FullApp/Client/
COPY FullApp/Shared/. ./FullApp/Shared/
COPY FullAppUT.FrontEnd/. /app/FullAppUT.FrontEnd/
COPY FullAppUT.Backend/. /app/FullAppUT.Backend/


# Restaurar dependencias para todos los proyectos
RUN dotnet restore FullApp.sln

# Compilar la aplicación
RUN dotnet build -c Release

# Publicar la aplicación
RUN dotnet publish FullApp/Server/FullApp.Server.csproj -c Release -o /app/publish

# Establecer la imagen base para el contenedor de la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Establecer el directorio de trabajo del contenedor
WORKDIR /app

# Copiar los archivos publicados del proyecto
COPY --from=build-env /app/publish .

# Exponer el puerto en el que se ejecuta la aplicación
EXPOSE 8080
EXPOSE 3306

# Ejecutar la aplicación al iniciar el contenedor
ENTRYPOINT ["dotnet", "FullApp.Server.dll"]
