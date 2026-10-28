/**
 * Theme Particle Effects Engine - shopacc
 * Supports: 'tet', 'christmas', 'halloween', 'summer'
 * Ultra-smooth 60fps, low CPU, auto-pausing when tab inactive
 */
(function (window, document) {
    'use strict';

    window.initThemeEffects = function (themeType) {
        if (!themeType || themeType === 'none' || themeType === 'default') return;

        // Clean up previous canvas if any
        var oldCanvas = document.getElementById('theme-particle-canvas');
        if (oldCanvas) oldCanvas.remove();

        var canvas = document.createElement('canvas');
        canvas.id = 'theme-particle-canvas';
        document.body.appendChild(canvas);

        var ctx = canvas.getContext('2d');
        if (!ctx) return;

        var width = (canvas.width = window.innerWidth);
        var height = (canvas.height = window.innerHeight);

        var isMobile = width < 768;
        var maxParticles = isMobile ? 22 : 45;
        var particles = [];
        var animationId = null;
        var isPaused = false;

        function random(min, max) {
            return Math.random() * (max - min) + min;
        }

        // --- Particle Factory ---
        function createParticle() {
            var p = {
                x: random(0, width),
                y: themeType === 'summer' ? random(height, height + 200) : random(-100, -10),
                size: random(isMobile ? 6 : 8, isMobile ? 14 : 20),
                speedY: themeType === 'summer' ? -random(0.8, 2.2) : random(0.8, 2.5),
                speedX: random(-1.2, 1.2),
                angle: random(0, Math.PI * 2),
                spin: random(-0.03, 0.03),
                opacity: random(0.5, 0.95),
                oscillation: random(0, Math.PI * 2),
                oscSpeed: random(0.015, 0.04),
                type: Math.random() > 0.5 ? 1 : 2
            };
            return p;
        }

        for (var i = 0; i < maxParticles; i++) {
            var p = createParticle();
            p.y = random(0, height); // Spread initially across screen
            particles.push(p);
        }

        // --- Draw Methods ---
        function drawTetPetal(p) {
            ctx.save();
            ctx.translate(p.x, p.y);
            ctx.rotate(p.angle);
            ctx.globalAlpha = p.opacity;

            ctx.beginPath();
            if (p.type === 1) {
                // Peach Blossom Petal (Hoa đào - Soft Pink / Rose)
                ctx.fillStyle = '#fb7185';
                ctx.moveTo(0, 0);
                ctx.bezierCurveTo(-p.size / 2, -p.size, -p.size / 2, -p.size * 1.5, 0, -p.size * 1.8);
                ctx.bezierCurveTo(p.size / 2, -p.size * 1.5, p.size / 2, -p.size, 0, 0);
            } else {
                // Apricot Blossom Petal (Hoa mai - Warm Gold)
                ctx.fillStyle = '#facc15';
                ctx.moveTo(0, 0);
                ctx.bezierCurveTo(-p.size / 2, -p.size * 0.8, -p.size / 1.8, -p.size * 1.4, 0, -p.size * 1.6);
                ctx.bezierCurveTo(p.size / 1.8, -p.size * 1.4, p.size / 2, -p.size * 0.8, 0, 0);
            }
            ctx.fill();
            ctx.restore();
        }

        function drawSnowflake(p) {
            ctx.save();
            ctx.translate(p.x, p.y);
            ctx.rotate(p.angle);
            ctx.globalAlpha = p.opacity;
            ctx.fillStyle = '#ffffff';

            if (p.type === 1) {
                // Detailed Star Snowflake
                ctx.strokeStyle = '#ffffff';
                ctx.lineWidth = Math.max(1, p.size / 7);
                for (var j = 0; j < 6; j++) {
                    ctx.beginPath();
                    ctx.moveTo(0, 0);
                    ctx.lineTo(0, p.size);
                    ctx.moveTo(0, p.size * 0.5);
                    ctx.lineTo(p.size * 0.3, p.size * 0.7);
                    ctx.moveTo(0, p.size * 0.5);
                    ctx.lineTo(-p.size * 0.3, p.size * 0.7);
                    ctx.stroke();
                    ctx.rotate(Math.PI / 3);
                }
            } else {
                // Soft glowing snow crystal dot
                var grad = ctx.createRadialGradient(0, 0, 0, 0, 0, p.size / 2);
                grad.addColorStop(0, 'rgba(255,255,255,0.95)');
                grad.addColorStop(0.5, 'rgba(224,242,254,0.7)');
                grad.addColorStop(1, 'rgba(255,255,255,0)');
                ctx.fillStyle = grad;
                ctx.beginPath();
                ctx.arc(0, 0, p.size / 2, 0, Math.PI * 2);
                ctx.fill();
            }
            ctx.restore();
        }

        function drawHalloweenBat(p) {
            ctx.save();
            ctx.translate(p.x, p.y);
            ctx.rotate(p.angle * 0.3);
            ctx.globalAlpha = p.opacity;

            if (p.type === 1) {
                // Flying silhouette bat
                var wingSpan = p.size * 1.4;
                var wingFlap = Math.sin(p.oscillation * 4) * 0.3;
                ctx.fillStyle = '#1e1b4b';

                ctx.beginPath();
                ctx.moveTo(0, 0);
                // Left Wing
                ctx.bezierCurveTo(-wingSpan * 0.5, -wingSpan * (0.6 + wingFlap), -wingSpan, 0, -wingSpan * 1.2, wingSpan * 0.3);
                ctx.bezierCurveTo(-wingSpan * 0.8, wingSpan * 0.2, -wingSpan * 0.4, wingSpan * 0.5, 0, wingSpan * 0.2);
                // Right Wing
                ctx.bezierCurveTo(wingSpan * 0.4, wingSpan * 0.5, wingSpan * 0.8, wingSpan * 0.2, wingSpan * 1.2, wingSpan * 0.3);
                ctx.bezierCurveTo(wingSpan, 0, wingSpan * 0.5, -wingSpan * (0.6 + wingFlap), 0, 0);
                ctx.fill();
            } else {
                // Floating Autumn Ember / Wisp
                var grad = ctx.createRadialGradient(0, 0, 0, 0, 0, p.size * 0.6);
                grad.addColorStop(0, 'rgba(249,115,22,0.9)');
                grad.addColorStop(0.6, 'rgba(168,85,247,0.5)');
                grad.addColorStop(1, 'rgba(0,0,0,0)');
                ctx.fillStyle = grad;
                ctx.beginPath();
                ctx.arc(0, 0, p.size * 0.6, 0, Math.PI * 2);
                ctx.fill();
            }
            ctx.restore();
        }

        function drawSummerParticle(p) {
            ctx.save();
            ctx.translate(p.x, p.y);
            ctx.globalAlpha = p.opacity;

            if (p.type === 1) {
                // Sparkling Beach Water Bubble
                ctx.strokeStyle = 'rgba(56,189,248,0.7)';
                ctx.lineWidth = 1.5;
                ctx.fillStyle = 'rgba(224,242,254,0.2)';
                ctx.beginPath();
                ctx.arc(0, 0, p.size / 2, 0, Math.PI * 2);
                ctx.fill();
                ctx.stroke();

                // Highlight shine on bubble
                ctx.fillStyle = '#ffffff';
                ctx.beginPath();
                ctx.arc(-p.size / 6, -p.size / 6, p.size / 7, 0, Math.PI * 2);
                ctx.fill();
            } else {
                // Warm Golden Sun Sparkle
                ctx.fillStyle = '#f59e0b';
                var s = p.size * 0.45;
                ctx.beginPath();
                ctx.moveTo(0, -s);
                ctx.quadraticCurveTo(0, 0, s, 0);
                ctx.quadraticCurveTo(0, 0, 0, s);
                ctx.quadraticCurveTo(0, 0, -s, 0);
                ctx.quadraticCurveTo(0, 0, 0, -s);
                ctx.fill();
            }
            ctx.restore();
        }

        // --- Main Animation Loop ---
        function update() {
            if (isPaused) return;

            ctx.clearRect(0, 0, width, height);

            for (var i = 0; i < particles.length; i++) {
                var p = particles[i];

                p.oscillation += p.oscSpeed;
                p.x += p.speedX + Math.sin(p.oscillation) * 0.8;
                p.y += p.speedY;
                p.angle += p.spin;

                if (themeType === 'tet') {
                    drawTetPetal(p);
                } else if (themeType === 'christmas') {
                    drawSnowflake(p);
                } else if (themeType === 'halloween') {
                    drawHalloweenBat(p);
                } else if (themeType === 'summer') {
                    drawSummerParticle(p);
                }

                // Recycle particle
                if (themeType === 'summer') {
                    if (p.y < -50 || p.x < -50 || p.x > width + 50) {
                        particles[i] = createParticle();
                        particles[i].y = height + random(10, 80);
                    }
                } else {
                    if (p.y > height + 50 || p.x < -50 || p.x > width + 50) {
                        particles[i] = createParticle();
                        particles[i].y = -random(10, 50);
                    }
                }
            }

            animationId = requestAnimationFrame(update);
        }

        update();

        // Window resize handler
        function onResize() {
            width = canvas.width = window.innerWidth;
            height = canvas.height = window.innerHeight;
        }
        window.addEventListener('resize', onResize);

        // Visibility change (pause when tab hidden to save CPU/battery)
        document.addEventListener('visibilitychange', function () {
            if (document.hidden) {
                isPaused = true;
                if (animationId) cancelAnimationFrame(animationId);
            } else {
                isPaused = false;
                update();
            }
        });
    };
})(window, document);
