> create a compose file
    -  one container at a time
    -  maintain the 10s delay betweeen updates

# Terminal command

1. deploy
- docker stack deploy -c docker-compose.yml web-stack

2. checking
- docker stack services web-stack

3. update  
- docker service update --image httpd:alpine web-stack_webapp