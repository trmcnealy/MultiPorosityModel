

// C file
//#include < emscripten.h >
//EMSCRIPTEN_KEEPALIVE
//int fib(int n) {
//    int i, t, a = 0, b = 1;
//    for (i = 0; i < n; i++) {
//        t = a + b;
//        a = b;
//        b = t;
//    }
//    return b;
//}
















(async function () {

    const imports = {
        env: {
            memory: new WebAssembly.Memory({initial: 1}),
            STACKTOP: 0,
        }
    };

    const { instance } = await WebAssembly.instantiateStreaming(fetch('/a.out.wasm'), imports);

    console.log(instance.exports._fib(12));

    // Make sure that your.wasm files have Content - Type: application / wasm.Otherwise they will be rejected by WebAssembly.
})();