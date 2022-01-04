#!/bin/bash

DIR="`dirname \"$0\"`"

echo "deleting build folder"
rm -rf $DIR/build

echo "Cleaning solution"
dotnet clean

# echo "Restoring dependencies"
dotnet restore

# echo "Building Solution"
dotnet publish RoslynWalker_DotNet5_Sample -c Release -r ubuntu.20.04-x64 /p:PublishSingleFile=true

mkdir $DIR/build
# echo "Copying published data to build folder"
rsync -rtvp $DIR/RoslynWalker_DotNet5_Sample/bin/Release/net5.0/ubuntu.20.04-x64/publish/* $DIR/build
rsync -rtvp $DIR/runner $DIR/build
