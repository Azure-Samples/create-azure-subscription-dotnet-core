# Programmatically create Azure EA Subscriptions with .NET Core

This is a simple .NET Core sample that uses the Azure .NET SDK to programmatically create EA subscriptions with a service principal.

## Run this sample

1. Get the [.NET Core SDK](https://www.microsoft.com/net/core).

1. Create an Azure service principal either through
    [Azure CLI](https://docs.microsoft.com/cli/azure/create-an-azure-service-principal-azure-cli?toc=%2fazure%2fazure-resource-manager%2ftoc.json),
    [PowerShell](https://docs.microsoft.com/azure/azure-resource-manager/resource-group-authenticate-service-principal/)
    or [the portal](https://docs.microsoft.com/azure/azure-resource-manager/resource-group-create-service-principal-portal/).

1. As an EA Account Owner, follow instructions to [give the service principal access to your enrollment account](https://aka.ms/easubcreationpublicpreview).

1. Clone the repository and install dependencies

    ```bash
    git clone https://github.com/Azure-Samples/create-azure-subscription-dotnet-core.git
    cd create-azure-subscription-dotnet-core
    dotnet restore
    ```

1. Create an `appsettings.json` using your subscription ID, tenant domain, client ID, and client secret from the service principle that you created. Example:

    ```json
    {
        "tenantId": "yourtenant.onmicrosoft.com",
        "appId": "app ID of the service principal",
        "secret": "client secret of the service principal"
    }
    ```

1. Run the sample.

    ```bash
    dotnet run
    ```

## Resources

* [Programmatically create Azure Enterprise subscriptions (preview)](https://aka.ms/easubcreationpublicpreview)
* [Azure Enterprise Agreement (EA)](https://azure.microsoft.com/pricing/enterprise-agreement/)
* [Azure EA Dev/Test option](https://azure.microsoft.com/offers/ms-azr-0148p/)
* [EA cost reporting API](https://docs.microsoft.com/azure/billing/billing-enterprise-api)
