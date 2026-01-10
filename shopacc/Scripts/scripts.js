//function createSnowflake() {
//    const snowflake = document.createElement('div');
//    snowflake.classList.add('snowflake');

//    // Random snowflake symbols
//    const symbols = ['❄', '❅', '❆', '✻', '✼', '❉'];
//    snowflake.innerHTML = symbols[Math.floor(Math.random() * symbols.length)];

//    snowflake.style.left = Math.random() * 100 + '%';
//    snowflake.style.fontSize = (Math.random() * 15 + 10) + 'px';
//    snowflake.style.opacity = Math.random() * 0.7 + 0.3;
//    snowflake.style.setProperty('--left-ini', Math.random() * 20 - 10 + 'vw');
//    snowflake.style.setProperty('--left-end', Math.random() * 20 - 10 + 'vw');
//    snowflake.style.animationDuration = (Math.random() * 8 + 5) + 's';
//    snowflake.style.animationDelay = Math.random() * 3 + 's';

//    document.body.appendChild(snowflake);

//    setTimeout(() => {
//        snowflake.remove();
//    }, 15000);
//}

// Create snowflakes periodically
setInterval(createSnowflake, 150);

// Initial snowflakes
for (let i = 0; i < 60; i++) {
    setTimeout(createSnowflake, i * 80);
}

//$('#menuToggle').click(function () {
//    $(this).toggleClass('active');
//    $('#mobileMenu').toggleClass('active');
//});

// Close mobile menu when clicking on a link
//$('#mobileMenu a').click(function () {
//    $('#menuToggle').removeClass('active');
//    $('#mobileMenu').removeClass('active');
//});

function show() {
    $(".header-mobile").addClass('active')
    $(".overflow").addClass('active')
}
function Close() {
    $(".header-mobile").removeClass('active')
    $(".overflow").removeClass('active')
}