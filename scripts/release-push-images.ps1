Param(
[string]$version="latest",
[string]$username,
[string]$password
)
	
docker login --username=$username --password $password

docker push gpasnik/microservices-api:latest
docker push gpasnik/microservices-api:$version

docker push gpasnik/microservices-remarks:latest
docker push gpasnik/microservices-remarks:$version

docker push gpasnik/microservices-statistics:latest
docker push gpasnik/microservices-statistics:$version

docker push gpasnik/microservices-storage:latest
docker push gpasnik/microservices-storage:$version

docker push gpasnik/microservices-users:latest
docker push gpasnik/microservices-users:$version

