# CampusEats — SE3090 Lab 11 (IT24103606)

**Live Demo:** [campuseats-fullstack-pubu.vercel.app](https://campuseats-fullstack-pubu.vercel.app)

A beginner-friendly React/Vite frontend and optional ASP.NET Core 8 API + Neon PostgreSQL database deployment scaffold for SLIIT's SE3090 DevOps and Cloud Hosting Platforms lab.

## Project structure

- `web/` — React/Vite application, sample menu fallback, Vercel config.
- `api/` — ASP.NET Core 8 minimal API with `/health`, `/api/menu`, `/api/menu/{id}` endpoints, CORS, Railway Dockerfile, and EF Core PostgreSQL migration.
- `.github/workflows/frontend-ci.yml` — GitHub Actions workflow for frontend builds.

## 1. Run the frontend locally

```bash
cd web
npm install
npm run dev
```

Visit the localhost URL shown by Vite. When `VITE_API_URL` is unset, the website uses `web/public/menu.json`. This is **demo mode**, not a live cloud database.

Test production build:

```bash
npm run build
npm run preview
```

## 2. Publish source to your own GitHub repository

Create `campuseats-fullstack` in the `pubudini-rathnayake` account. Upload the files and folders **inside this ZIP** to the repository root. **Do not upload the ZIP as a single file**. Commit them to `main`. `web/` and `api/` must be visible at the repository root.

## 3. Deploy the frontend (required for Lab 11)

1. Go to https://vercel.com/new and import your `campuseats-fullstack` GitHub repo.
2. Set **Root Directory** to `web` (critical for this monorepo).
3. Framework: Vite; Install: `npm install`; Build: `npm run build`; Output: `dist`.
4. For the first deployment, leave `VITE_API_URL` **unset** so the sample menu loads.
5. Deploy. Check the site and save screenshots of the successful deployment, build logs, and live site.
6. After the backend is deployed, set `VITE_API_URL` to the live **HTTPS** API base URL (no trailing slash) and **redeploy**. The setting is public configuration, not a secret.

Note: the lab asks for evidence of configuring `VITE_API_URL`. If backend isn't deployed in time, you can document the variable conceptually, then configure it once there is a valid backend URL. A placeholder URL will cause live menu API calls to fail; avoid claiming working end-to-end integration.

## 4. Deploy the backend (optional; Lab 11 permits documenting this)

Prerequisite: have an accessible ASP.NET Core 8 Docker host (Railway/Render). In Railway:

1. New Project -> Deploy from GitHub -> this repo.
2. Service Root Directory = `/api`; Dockerfile auto-detected in that folder.
3. Add `Cors__AllowedOrigins` set to your real Vercel URL (no trailing slash).
4. Set PORT to `8080` or the platform's assigned port. API binds to `0.0.0.0:PORT` when provided.
5. If you have Neon PostgreSQL, add `ConnectionStrings__Default` from its credentials in **Npgsql key=value format** with `SSL Mode=Require`. Do not commit this value.
6. Deploy and generate an HTTPS domain. Test `/health` and `/api/menu`; database mode needs correct credentials and migrations.
7. Set Vercel `VITE_API_URL` to that public API base URL and redeploy the frontend. Test the menu.

The API intentionally provides sample data when no connection string is set; in that mode `/health` returns `"database":"demo"`, and it is **not evidence of Neon connectivity**. With Neon configured, EF Core calls `Database.Migrate()` on startup and seeds a menu if empty. Production systems usually run migrations as a controlled release step, but startup migrations simplify this teaching demo.

## 5. Evidence and reporting

Capture **your own** screenshots for GitHub, build result, Vercel project, live site, deploy log, backend/API if attempted, Neon dashboard and variables (values hidden). Report successes and incomplete tasks accurately.

## Environment variable summary

- Frontend: `VITE_API_URL` (optional until backend URL exists; publicly visible in built client code).
- Backend: `Cors__AllowedOrigins`, `PORT`, and `ConnectionStrings__Default` (secret; only when using Neon).
- The lab sheet also discusses `Jwt__Key`, `Jwt__Issuer`, `Jwt__Audience`. This starter API **does not implement login/JWT authentication**, so those settings are not used. Document them as variables needed **if authentication from Labs 03–04 is added**.

## Caution

This repository is a newly generated Lab 11 starter, **not the source code of your previous Lab 02**. No GitHub upload or live deployment has been performed automatically. Do not reuse another student's deployment screenshots or claim their URLs are your own.
