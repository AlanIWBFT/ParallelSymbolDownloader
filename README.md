Parallel Symbol Downloader
==========================
When symbol servers are enabled in IDEs, the first launch can be painfully slow (especially JetBrains Rider) because they download symbol files (.pdb)s sequentially. When debugging applications with many modules (like, Unreal Engine) this is simply unbearable. Inspired by Superluminal Performance's parallel symbol resolver, this utility allows you to run multiple symbol downloads in parallel.

Syntax
------
Right now only the underlying worker is done so we need to launch multiple workers manually. That being said, launching the workers one by one is clearly not so great so we're going use a `for /l` loop in `cmd` to do so. A GUI is planned. 

`symbol-download-worker.exe pID workerId numTotalWorkers path-to-symbol-cache-and-server`

An example, assuming UnrealEditor.exe's PID is 37388, and we want to download 10 symbols in parallel (which means 10 workers) from `https://msdl.microsoft.com/download/symbols` into local cache `F:\SymbolCache`, the command line for worker #0 will look like:

`symbol-download-worker.exe 37388 0 10 cache*F:\SymbolCache;srv*https://msdl.microsoft.com/download/symbols`

Notice how the local cache location is prepended with `cache*` and the server is prepended with `srv*` and how they are separated by a semicolon `;`.

Parallel Launch
---------------
Start `cmd` shell and execute

`for /l %x in (0, 1, 9) do start symbol-download-worker.exe 37388 %x 10 cache*F:\SymbolCache;srv*https://msdl.microsoft.com/download/symbols`

will launch 10 workers (numbered 0-9) populating your symbol cache.