# Already the above task  contains 3 replicas i just need to scalle up to 5

- docker service scale  nginx-stack_nginx-web=5

## checking
- docker stack services nginx-stack