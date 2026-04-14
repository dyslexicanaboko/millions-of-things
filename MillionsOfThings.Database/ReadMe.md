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

## Accessing Postgres via DBeaver

2026-04-14

DBeaver has a completely different approach to how you work with your database, it's not intuitive at all, and borderline infuriating. That being said it's not browser based which is why I started using it instead of pgAdmin. It's so unclear how it's used I have to write these instructions here so I can remember how to work with my own SQL files after enough time has passed.

1. Open up the workspace which in my case is the `MillionsOfThings.Database` folder.
2. I setup the workspace to link back to folders I established already with my scripts.
3. Even after opening those files, they won't have a connection to the database immediately. You have to click on the "Active Datasource" dropdown which is unamed to the right of the padlock.
   1. The padlock is to make the datasource readonly, so you can hover over the icons until you find that.
   2. Do not be tempted to click on the green "Connect" powerplug icon because it won't do anything and provides zero feedback.
4. Once you select the "Active Datasource", then and only then can you run queries.
   1. You can highlight your line and press `CTRL` + `Enter` to run queries because apparently `F5` or `F9` was too logical a choice.

### Changing execution keyboard shortcut

The default execution keyboard shortcut is `CTRL` + `Enter`. It can be changed to `F5` by following these steps:

1. Open "Window > Preferences"
2. Navigate to "User Interface > Keys"
3. In the search bar type "execute"
4. Change the "Execute SQL query" to `F5`

The only caution is that `F5` is refresh in most parts of the application.
