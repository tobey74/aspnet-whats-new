ARG VARIANT=1-8.0-bookworm
FROM mcr.microsoft.com/devcontainers/dotnet:${VARIANT}

# Install .NET 10.0 nightly
RUN wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh \
    && chmod +x dotnet-install.sh \
    && sudo ./dotnet-install.sh --version 10.0.100-preview.1.25104.26 --install-dir /usr/share/dotnet \
    && rm dotnet-install.sh