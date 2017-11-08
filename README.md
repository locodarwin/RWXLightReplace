# RWXLightReplace
For replacing Surface, Ambient, and Diffuse script tags on bulk RWX objects.

* Point it at a directory of RWX files, zipped or unipped
* Will scan each file for Surface, Ambient, or Diffuse tags and change their values
* Option to re-zip the files after processing

#Caveats
RWXLightReplace won't unzip a file if another file with the same name is in the processing directory. For example, if you have a file named test.zip that contains a file called test.rwx, then it will not extract if there is already a test.rwx file in the directory. Will be fixed in a later version.

Will not process password protected zips. Unzip those first with a program like multizip.
