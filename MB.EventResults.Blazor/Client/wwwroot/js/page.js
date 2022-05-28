window.downloadFromByteArray = function (options) {
  // Convert base64 string to numbers array.
  const numArray = atob(options.byteArray).split('').map(c => c.charCodeAt(0));

  // Convert numbers array to Uint8Array object.
  const uint8Array = new Uint8Array(numArray);

  // Wrap it by Blob object.
  const blob = new Blob([uint8Array], { type: options.contentType });

  // Create "object URL" that is linked to the Blob object.
  const url = URL.createObjectURL(blob);

  // Invoke download helper function that implemented in 
  // the earlier section of this article.
  downloadFromUrl({ url: url, fileName: options.fileName });

  // At last, release unused resources.
  URL.revokeObjectURL(url);
}

window.downloadFromUrl = function (options) {
  const anchorElement = document.createElement('a');
  anchorElement.href = options.url;
  anchorElement.download = options.fileName ?? '';
  anchorElement.click();
  anchorElement.remove();
}

// loadScript: returns a promise that completes when the script loads
window.loadScript = function (scriptPath) {
  // check list - if already loaded we can ignore
  if (loaded[scriptPath]) {
    console.log(scriptPath + " already loaded");
    // return 'empty' promise
    return new this.Promise(function (resolve, reject) {
      resolve();
    });
  }

  return new Promise(function (resolve, reject) {
    // create JS library script element
    var script = document.createElement("script");
    script.src = scriptPath;
    script.type = "text/javascript";
    console.log(scriptPath + " created");

    // flag as loading/loaded
    loaded[scriptPath] = true;

    // if the script returns okay, return resolve
    script.onload = function () {
      console.log(scriptPath + " loaded ok");
      resolve(scriptPath);
    };

    // if it fails, return reject
    script.onerror = function () {
      console.log(scriptPath + " load failed");
      reject(scriptPath);
    }

    // scripts will load at end of body
    document["body"].appendChild(script);
  });
}
// store list of what scripts we've loaded
loaded = [];