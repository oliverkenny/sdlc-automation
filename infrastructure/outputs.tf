output "pages_project_name" {
  description = "Cloudflare Pages project name"
  value       = cloudflare_pages_project.team_dashboard.name
}

output "pages_project_subdomain" {
  description = "Cloudflare Pages default subdomain"
  value       = "${cloudflare_pages_project.team_dashboard.name}.pages.dev"
}
