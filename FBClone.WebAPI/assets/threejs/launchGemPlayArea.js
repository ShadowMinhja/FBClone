function init() {

    canvas = document.getElementById("c");
    if (canvas == null)
        return; //Escape
    var geometries = [
        new THREE.SphereGeometry(0.5, 12, 8),
        new THREE.DodecahedronGeometry(0.5)
    ];

    var template = document.getElementById("template").text;
    content = document.getElementById("content-gem");
    if (content === null)
        return; //Escape

    for (var i = 0; i < 5; i++) {

        var scene = new THREE.Scene();

        // make a list item
        var element = document.createElement("div");
        element.className = "list-item";
        element.innerHTML = template.replace('$', i + 1);

        // Look up the element that represents the area
        // we want to render the scene
        scene.userData.element = element.querySelector(".scene");
        content.appendChild(element);

        var camera = new THREE.PerspectiveCamera(50, 1, 1, 10);
        camera.position.z = 2;
        scene.userData.camera = camera;

        var controls = new THREE.OrbitControls(scene.userData.camera, scene.userData.element);
        controls.minDistance = 2;
        controls.maxDistance = 5;
        controls.enablePan = false;
        controls.enableZoom = false;
        scene.userData.controls = controls;

        // add one random mesh to each scene
        var geometry = geometries[geometries.length * Math.random() | 0];

        var material = new THREE.MeshStandardMaterial({

            color: new THREE.Color().setHSL(Math.random(), 1, 0.75),
            roughness: 0.5,
            metalness: 0,
            shading: THREE.FlatShading

        });

        scene.add(new THREE.Mesh(geometry, material));

        scene.add(new THREE.HemisphereLight(0xaaaaaa, 0x444444));

        var light = new THREE.DirectionalLight(0xffffff, 0.7);
        light.position.set(1, 1, 1);
        scene.add(light);

        scenes.push(scene);

    }


    renderer = new THREE.WebGLRenderer({ canvas: canvas, antialias: true });
    renderer.setClearColor(0xffffff, 1);
    renderer.setPixelRatio(window.devicePixelRatio);

}

function updateSize() {
    if (canvas == null)
        return; //Escape
    var width = canvas.clientWidth;
    var height = canvas.clientHeight;

    if (canvas.width !== width || canvas.height != height) {
        renderer.setSize(width, height, false);
    }

    if (content.clientWidth !== width || content.clientHeight != height) {
        content.clientWidth = width;
        content.clientHeight = height;
    }
}

function animate() {

    render();
    requestAnimationFrame(animate);

}

function render() {

    updateSize();

    if (renderer == null)
        return; //Escape
    renderer.setClearColor(0xffffff);
    renderer.setScissorTest(false);
    renderer.clear();

    renderer.setClearColor(0xffffff);
    renderer.setScissorTest(true);

    scenes.forEach(function (scene) {

        // so something moves
        scene.children[0].rotation.y = Date.now() * 0.001;

        // get the element that is a place holder for where we want to
        // draw the scene
        var element = scene.userData.element;

        // get its position relative to the page's viewport
        var rect = element.getBoundingClientRect();

        // check if it's offscreen. If so skip it
        if (rect.bottom < 0 || rect.top > renderer.domElement.clientHeight ||
             rect.right < 0 || rect.left > renderer.domElement.clientWidth) {

            return;  // it's off screen

        }

        // set the viewport
        var width = rect.right - rect.left;
        var height = rect.bottom - rect.top;
    
        var left = rect.left;
        var bottom = 174 + renderer.domElement.clientHeight - rect.bottom; //275;
        renderer.setViewport(left, bottom, width, height);
        renderer.setScissor(left, bottom, width, height);

        var camera = scene.userData.camera;

        //camera.aspect = width / height; // not changing in this example
        //camera.updateProjectionMatrix();

        //scene.userData.controls.update();

        renderer.render(scene, camera);

    });

}

if (!Detector.webgl) Detector.addGetWebGLMessage();

var canvas;

var scenes = [], renderer;

init();
animate();