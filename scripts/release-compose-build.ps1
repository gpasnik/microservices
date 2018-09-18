Param(
[string]$version="latest"
)

docker-compose -f "../src/GP.Microservices/docker-compose.release.yml" build

docker tag gpasnik/microservices-api:latest gpasnik/microservices-api:$version

docker tag gpasnik/microservices-users:latest gpasnik/microservices-users:$version

docker tag gpasnik/microservices-remarks:latest gpasnik/microservices-remarks:$version

docker tag gpasnik/microservices-statistics:latest gpasnik/microservices-statistics:$version

docker tag gpasnik/microservices-storage:latest gpasnik/microservices-storage:$version
