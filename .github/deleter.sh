#!/bin/bash

git config --local user.name "Branch Bot"
branches=$(git branch -r --merged | grep -v origin/release | grep -v origin/master | grep -v origin/develop | grep -v HEAD | sed 's/origin\///g')
readarray -t y < <(printf '%s' "$branches")

for i in "${y[@]}"
do
	echo "Deleting Branch $i"
	git push --delete origin $i
done
