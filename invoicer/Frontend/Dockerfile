# Frontend/Dockerfile
ARG DOTNET_VERSION=8.0
ARG NGINX_VERSION=alpine

# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Frontend/", "Frontend/"]
COPY ["Application/", "Application/"]
COPY ["Domain/", "Domain/"]
COPY ["Shared/", "Shared/"]

RUN dotnet restore "Frontend/Frontend.csproj"

WORKDIR "/src/Frontend"
RUN dotnet publish "Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore

# --- Final Stage (Nginx to serve static files) ---
FROM nginx AS final

RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY Frontend/nginx.conf /etc/nginx/nginx.conf
RUN chown -R nginx:nginx /usr/share/nginx/html && chmod -R 755 /usr/share/nginx/html

EXPOSE 80
