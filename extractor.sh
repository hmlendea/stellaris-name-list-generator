#!/bin/bash

FILE="ui-name-lists/common/name_lists/ui_extra_humans_extended.txt"

X=$1
Y=$2
INDENTATION=$3

tail -n +$X $FILE | head -n $((Y-X+1)) | sed 's/\r//g' | sed 's/^ *//g' | sed 's/#.*$//g' \
    | sed 's/\"\([^\"\ ][^\"\ ]*\)\"/\1/g' \
    | sed 's/\"\([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\"/\1@\2/g' \
    | sed 's/\"\([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\"/\1@\2@\3/g' \
    | sed 's/\"\([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\"/\1@\2@\3@\4/g' \
    | sed 's/\"\([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\"/\1@\2@\3@\4/g' \
    | sed 's/\"\([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\ \([^\"\ ][^\"\ ]*\)\"/\1@\2@\3@\4@\5/g' \
    | sed 's/  */ /g' \
    | sed 's/ /\n/g' \
    | sort | uniq \
    | sed -r '/^\s*$/d' \
    | sed 's/@/ /g' \
    | sed 's|\(.*\)|'"$INDENTATION"'<string>\1</string>|g' \
    | sort > result.txt

