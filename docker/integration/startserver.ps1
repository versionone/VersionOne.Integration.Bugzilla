robocopy "..\" ".\tmp" setup_bugzilla.cfg
robocopy "..\" ".\tmp" setup_mysql.bash

docker build --tag bugzillav1integration:1.0 .
docker run --publish 8088:80 --detach --name bugzillav1 bugzillav1integration:1.0
# docker run --publish 8088:80 --name bugzillav1 bugzillav1integration:1.0
Start-Sleep -s 15
docker exec bugzillav1 /var/www/html/bugzilla/setup_mysql.bash
docker exec bugzillav1 /var/www/html/bugzilla/checksetup.pl setup_bugzilla.cfg