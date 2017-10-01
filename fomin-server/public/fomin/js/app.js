var requestAnimFrame = (function () {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (callback) {
            window.setTimeout(callback, 1000 / 60);
        };
})();

var canvas = document.createElement("canvas");
var ctx = canvas.getContext("2d");
canvas.width = document.body.clientWidth;
canvas.height = document.body.clientHeight + 5;
document.body.appendChild(canvas);

var lastTime;

function main() {
    var now = Date.now();
    var dt = (now - lastTime) / 1000.0;

    update(dt);
    render();

    lastTime = now;
    requestAnimFrame(main);
}

window.onresize = function (event) {
    canvas.width = document.body.clientWidth;
    canvas.height = document.body.clientHeight + 5;
};

function init() {
    // terrainPattern = ctx.createPattern(resources.get('img/terrain.png'), 'repeat');

    document.getElementById('play-again').addEventListener('click', function () {
        reset();
    });

    //reset();
    lastTime = Date.now();
    main();
}

resources.load([
    'img/fomin.png',
    'img/barrel.png'
]);
resources.onReady(init);

var fomin = {
    pos: [canvas.width / 2 - 78 / 2, canvas.height / 2 - 111 / 2],
    sprite: new Sprite('img/fomin.png', [0, 0], [78, 111], 2, [0])
};

var barrels = [{
    pos: [canvas.width,
        Math.floor(Math.random() * (canvas.height - 39))],
    sprite: new Sprite('img/barrel.png', [0, 0], [74, 94],
        2, [0])
}];

var gameTime = 0;
var isGameOver;
var fominSpeed = 200;
var barrelSpeed = 70;

var score = 0;
var scoreEl = document.getElementById('score');

function update(dt) {
    gameTime += dt;

    handleInput(dt);
    updateEntities(dt);

    barrelSpeed = 70 + gameTime / 2;

    // It gets harder over time by adding enemies using this
    // equation: 1-.993^gameTime
    if (Math.random() < 1 - Math.pow(.9999, gameTime)) {
        for (var i = 0; i < 10; i++) {
            var barrel = {
                pos: [canvas.width,
                    Math.random() > 0.5 ? fomin.pos[1] - 47 : Math.floor(Math.random() * (canvas.height - 39)) + 2],
                sprite: new Sprite('img/barrel.png', [0, 0], [74, 94],
                    2, [0])
            };

            if (isBarrelCollidesWithOtherBarrels(barrel)) continue;

            barrels.push(barrel);
            break;
        }
    }

    checkCollisions();

    if (!isGameOver)
        scoreEl.innerHTML = Math.floor(score / 10);
}

function updateEntities(dt) {
    // Update the player sprite animation
    fomin.sprite.update(dt);

    for (var i = 0; i < barrels.length; i++) {
        barrels[i].pos[0] -= barrelSpeed * dt;
        barrels[i].sprite.update(dt);

        // Remove if offscreen
        if (barrels[i].pos[0] + barrels[i].sprite.size[0] < 0) {
            barrels.splice(i, 1);
            i--;
        }
    }
}

function render() {
    //ctx.fillStyle = terrainPattern;
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // Render the player if the game isn't over
    if (!isGameOver) {
        renderEntity(fomin);
    }

    renderEntities(barrels);
}

function renderEntities(list) {
    for (var i = 0; i < list.length; i++) {
        renderEntity(list[i]);
    }
}

function renderEntity(entity) {
    ctx.save();
    ctx.translate(entity.pos[0], entity.pos[1]);
    entity.sprite.render(ctx);
    ctx.restore();
}

function handleInput(dt) {
    if (input.isDown('DOWN') || input.isDown('s')) {
        fomin.pos[1] += fominSpeed * dt;
    }

    if (input.isDown('UP') || input.isDown('w')) {
        fomin.pos[1] -= fominSpeed * dt;
    }

    if (input.isDown('LEFT') || input.isDown('a')) {
        fomin.pos[0] -= fominSpeed * dt;
    }

    if (input.isDown('RIGHT') || input.isDown('d')) {
        fomin.pos[0] += fominSpeed * dt;
    }
}

function collides(x, y, r, b, x2, y2, r2, b2) {
    return !(r <= x2 || x > r2 ||
        b <= y2 || y > b2);
}

function barrelCollides(pos, size, pos2, size2) {
    return collides(pos[0], pos[1],
        pos[0] + size[0], pos[1] + size[1],
        pos2[0], pos2[1],
        pos2[0] + size2[0], pos2[1] + size2[1]);
}

function isBarrelCollidesWithOtherBarrels(barrel) {
    for (var i = 0; i < barrels.length; i++) {
        if (barrelCollides(barrel.pos, barrel.sprite.size, barrels[i].pos, barrels[i].sprite.size)) {
            return true;
        }
    }
    return false;
}

function checkCollisions() {
    checkFominBounds();

    // Run collision detection for all enemies and bullets
    for (var i = 0; i < barrels.length; i++) {
        var pos = barrels[i].pos;
        var size = barrels[i].sprite.size;

        if (barrelCollides(pos, size, fomin.pos, fomin.sprite.size)) {
            gameOver();
        }
        score += 1;
    }
}

function gameOver() {
    document.getElementById('game-over').style.display = 'block';
    document.getElementById('game-over-overlay').style.display = 'block';
    isGameOver = true;
}

function reset() {
    document.getElementById('game-over').style.display = 'none';
    document.getElementById('game-over-overlay').style.display = 'none';
    isGameOver = false;
    gameTime = 0;
    score = 0;

    barrels = [];
    barrels.push({
        pos: [canvas.width,
            Math.floor(Math.random() * (canvas.height - 39))],
        sprite: new Sprite('img/barrel.png', [0, 0], [74, 94],
            2, [0])
    });

    fomin.pos = [50, canvas.height / 2];
}

function checkFominBounds() {
    // Check bounds
    if (fomin.pos[0] < 0) {
        fomin.pos[0] = 0;
    }
    else if (fomin.pos[0] > canvas.width - fomin.sprite.size[0]) {
        fomin.pos[0] = canvas.width - fomin.sprite.size[0];
    }

    if (fomin.pos[1] < 0) {
        fomin.pos[1] = 0;
    }
    else if (fomin.pos[1] > canvas.height - fomin.sprite.size[1]) {
        fomin.pos[1] = canvas.height - fomin.sprite.size[1];
    }
}