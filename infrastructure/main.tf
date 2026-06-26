terraform {
  required_version = ">= 1.5"

  required_providers {
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = "~> 4.0"
    }
  }
}

provider "cloudflare" {
  api_token = var.cloudflare_api_token
}

resource "cloudflare_pages_project" "team_dashboard" {
  account_id        = var.cloudflare_account_id
  name              = "team-dashboard"
  production_branch = "main"

  source {
    type = "github"
    config {
      owner                         = "oliverkenny"
      repo_name                     = "sdlc-automation"
      production_branch             = "main"
      deployments_enabled           = false
      pr_comments_enabled           = false
      production_deployment_enabled = false
      preview_deployment_setting    = "none"
    }
  }

  build_config {
    build_command   = "dotnet publish src/TeamDashboard/TeamDashboard.csproj -c Release -o publish"
    destination_dir = "publish/wwwroot"
  }
}
