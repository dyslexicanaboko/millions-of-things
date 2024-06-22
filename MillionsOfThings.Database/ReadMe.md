# Setup

Using Postgres to get practice and learning purposes.

## Docker

I opted to install Postgres and it's accompanying IDE/Viewer via docker. I chose pgadmin4 as I have used it before and it works well. Here are the commands for setting that all up:

### Pulling images

Supporting links for the docker images and how to documentation:

- [Postgres image and docs](https://hub.docker.com/_/postgres)
- [pgAdmin4 image](https://hub.docker.com/r/dpage/pgadmin4)
- [pgAdmin4 docs](https://www.pgadmin.org/docs/pgadmin4/development/container_deployment.html)

```bash
docker pull postgres
docker pull dpage/pgadmin4
```

### Creating containers

I am using the default username and password because this is just for local development purposes. That is `postgres`, but obviously don't use this in a production environment. Additionally, I have my containers setup to not start automatically because I don't intend on using Postgres full time. This way I am guaranteed they are shut down on a restart of docker or of my computer.

```bash
docker run -p 5432:5432 --name postgres -e POSTGRES_PASSWORD=postgres -d postgres
docker run -p 88:80 --name pgadmin4 -e PGADMIN_DEFAULT_EMAIL=p@p.com -e PGADMIN_DEFAULT_PASSWORD=postgres -d dpage/pgadmin4
```

## Accessing Postgres via pgAdmin4

Assuming you have followed the exact instructions above then you should login with:

- username: p@p.com
- password: postgres

Once the dashboard loads, you should be asked to register a server. Here are the settings I provided:

- General tab
  - Name: localhost
  - Port: 5432
- Connection tab
  - Host name/address: host.docker.internal
  - Username: postgres
  - Password: postgres
  - Save password?: On

Press the `Save` button and you should be connected.

### About the host

It's important to notice the difference of the host used here. It's not `127.0.0.1` because it's hosted in the docker container so you must use that host name instead. If you are on Windows this is defined in your `hosts` file and it looks like this:

```text
# Added by Docker Desktop
192.168.157.128 host.docker.internal
192.168.157.128 gateway.docker.internal
# To allow the same kube context to work on the host and the container:
127.0.0.1 kubernetes.docker.internal
# End of section
```

This is an excerpt from `C:\Windows\System32\drivers\etc\hosts`.
