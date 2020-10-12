#!/bin/bash
mysql -e "create user bugs@localhost identified by 'bugs';"
mysql -e "create database bugs;"
mysql -e "grant all privileges on bugs.* to bugs;"