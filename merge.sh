#!/bin/bash

OUTPUT_FILE_NAME="names.xml"

head -2 $1 > "${OUTPUT_FILE_NAME}"

for FILE in "$@"; do
    tail -n +3 "${FILE}" | head -n -1 >> "${OUTPUT_FILE_NAME}"
    echo " " >> "${OUTPUT_FILE_NAME}"
done

tail -1 $1 >> "${OUTPUT_FILE_NAME}"
