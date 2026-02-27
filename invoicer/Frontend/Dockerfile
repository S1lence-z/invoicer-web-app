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
FROM nginx:${NGINX_VERSION} AS final

RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY Frontend/nginx.conf /etc/nginx/nginx.conf
COPY Frontend/entrypoint.sh /docker-entrypoint.d/40-configure-app.sh
RUN sed -i 's/\r$//' /docker-entrypoint.d/40-configure-app.sh && \
    chmod +x /docker-entrypoint.d/40-configure-app.sh

EXPOSE 80
