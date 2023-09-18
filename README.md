To whoever is reading this,

There are ways to improve the current architecture and implementation in general, including 
writing a docker-compose.yml, but the codebase is big enough as it is for a take-home assignment.

# How to run:

- Spin up the DB
````
docker pull postgres
docker run -d --name postgresCont -p 5432:5432 -e POSTGRES_PASSWORD=root postgres
````

- Apply migrations manually, stored in `/src/4Create.DotNet.Persistence/Migrations/`

- Run 

