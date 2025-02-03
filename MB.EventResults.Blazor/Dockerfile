FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .
WORKDIR /src/Server
RUN dotnet restore "MB.EventResults.Blazor.Server.csproj"
RUN dotnet publish
RUN export PATH="$PATH:/root/.dotnet/tools" && \
  dotnet new install Amazon.Lambda.Templates && \
  dotnet tool install -g --verbosity detailed Amazon.Lambda.Tools && \
  dotnet workload install wasm-tools && \
  dotnet workload list && \
  dotnet lambda package && \
  dotnet lambda package --output output/ MB.EventResults.Blazor.Server.csproj 

FROM public.ecr.aws/lambda/dotnet:8
WORKDIR /var/task
COPY --from=build /src/output/ MB.EventResults.Blazor.Server.zip .
CMD [ "MB.EventResults.Blazor.Server" ]