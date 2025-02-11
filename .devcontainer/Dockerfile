ARG VARIANT=1-8.0-bookworm
FROM mcr.microsoft.com/devcontainers/dotnet:${VARIANT}

# TODO: Find a way to get the latest .NET 10.0 preview 1 version
ARG VERSION="10.0.100-preview.1.25110.2"

# Install .NET 10.0 preview
RUN wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh \
    && chmod +x dotnet-install.sh \
    && sudo ./dotnet-install.sh --version ${VERSION} --install-dir /usr/share/dotnet \
    && rm dotnet-install.sh