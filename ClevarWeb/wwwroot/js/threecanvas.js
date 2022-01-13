import * as THREE from '../lib/threejs/build/three.module.js';
import { OrbitControls } from '../lib/threejs/libs/jsm/controls/OrbitControls.js';
import { ConvexBufferGeometry } from '../lib/threejs/libs/jsm/geometries/ConvexGeometry.js';

var group, camera, scene, renderer;

init();
animate();

function init() {

    scene = new THREE.Scene();

    renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
    renderer.setPixelRatio(window.devicePixelRatio);
    renderer.setSize(window.innerWidth, window.innerHeight);

    document.getElementById('threecanvas').appendChild(renderer.domElement);

    // camera

    camera = new THREE.PerspectiveCamera(40, window.innerWidth / window.innerHeight, 1, 1000);
    camera.position.set(15, 20, 30);
    scene.add(camera);

    // controls

    var controls = new OrbitControls(camera, renderer.domElement);


    scene.add(new THREE.AmbientLight(0x93A7C6));
    scene.background = new THREE.Color(0xf0f0f0);

    // light

    var light = new THREE.PointLight(0x93A7C6, 1);
    camera.add(light);

    // textures

    // var loader = new THREE.TextureLoader();
    // var texture = loader.load( 'textures/sprites/disc.png' );

    group = new THREE.Group();
    scene.add(group);

    // points

    var vertices = new THREE.DodecahedronGeometry(15).vertices;

    for (var i = 0; i < vertices.length; i++) {

        //vertices[ i ].add( randomPoint().multiplyScalar( 2 ) ); // wiggle the points

    }

    // convex hull

    var meshMaterial = new THREE.MeshLambertMaterial({
        color: 0xffffff,
        opacity: 0.5,
        transparent: true,
        // wireframe: true
    });

    var meshGeometry = new ConvexBufferGeometry(vertices);

    var mesh = new THREE.Mesh(meshGeometry, meshMaterial);
    mesh.material.side = THREE.BackSide; // back faces
    mesh.renderOrder = 0;
    group.add(mesh);

    var mesh = new THREE.Mesh(meshGeometry, meshMaterial.clone());
    mesh.material.side = THREE.FrontSide; // front faces
    mesh.renderOrder = 1;
    group.add(mesh);

    //

    window.addEventListener('resize', onWindowResize, false);

    camera.position.y += 0;
    camera.position.x -= 0;
    camera.position.z -= 15;

}

function onWindowResize() {

    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();

    renderer.setSize(window.innerWidth, window.innerHeight);

}

function animate() {

    requestAnimationFrame(animate);

    group.rotation.y += 0.0015;

    render();

}

function render() {

    renderer.render(scene, camera);

}