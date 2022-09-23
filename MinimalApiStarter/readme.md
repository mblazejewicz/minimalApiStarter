## docker cheatsheet
```sh 
docker ps 
```

```sh 
docker ps -a 
```

```sh docker rm containerId ```

## build docker image with version and latest tags
```sh 
docker build -t minimalapistarter:1.0.1 -t minimalapistarter:latest . 
```


##run docker image in conatainer
```powershell 
docker run -d -p 8080:80 --name myapp minimalapistarter:1.0.1 
```

``` -d ``` - detached
``` -it ``` - interactive
``` --rm ``` - flag to remove docker container on stop 

##trouble shooting docker image
####run an interactive shell container using that image and explore whatever content that image has.
```sh 
docker run -it image_name sh 
```

####for images with an entrypoint
```sh docker run -it --entrypoint sh image_name ```

####how the image was build (steps)
```sh docker image history --no-trunc image_name > image_history ```
####steps will be logged into the image_history file.

## stop container
```sh docker stop container_name ```