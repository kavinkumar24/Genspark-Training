Two ways to update service
1. create a docker compose file and in that modify
- docker stack deploy -c docker-compose.yml nginx-stack

2. update though command
- docker service update --image nginx: alpine nginx-web