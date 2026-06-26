# Infrastructure

Terraform configuration for the Team Dashboard Cloudflare Pages deployment.

## Prerequisites

- [Terraform](https://www.terraform.io/downloads) >= 1.5
- Cloudflare API token with Pages permissions
- Cloudflare account ID

## Usage

```bash
cd infrastructure

# Set credentials
export TF_VAR_cloudflare_account_id="your-account-id"
export TF_VAR_cloudflare_api_token="your-api-token"

# Or on PowerShell
$env:TF_VAR_cloudflare_account_id = "your-account-id"
$env:TF_VAR_cloudflare_api_token = "your-api-token"

# Initialise and apply
terraform init
terraform plan
terraform apply
```

## Importing Existing Resources

If the Cloudflare Pages project already exists:

```bash
terraform import cloudflare_pages_project.team_dashboard <account-id>/<project-name>
```

## Resources

| Resource | Description |
|----------|-------------|
| `cloudflare_pages_project.team_dashboard` | Cloudflare Pages project linked to the GitHub repository |

## Notes

- Deployments are handled via GitHub Actions (not Cloudflare's built-in git integration)
- The `deployments_enabled` flag is set to `false` to avoid duplicate deployments
- Build configuration is defined here for documentation but the actual build happens in CI
