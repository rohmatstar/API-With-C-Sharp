let main_url = "https://pokeapi.co/api/v2/pokemon/"
getPoke(main_url)
function getPoke(url) {
    $.ajax({
        url: url
    }).done(res => {
        if (res.next != null) {
            $("#next_page").removeClass("disabled").attr("onclick", "getPoke('"+res.next+"')");
        }
        res.results.forEach(function (data, index) {
            //console.log(data.url); // detail url

            $.ajax({
                url: data.url
            }).done((res) => {
                let gradient = setGradientBackground(2);
                console.log(gradient)
                let poke = `<div class="col-md-2 pe-0">
                <div class="card mb-2">
                    <div id="poke-wrapper" class="card-body position-relative">
                        <img class="poke" src="${res.sprites.other.dream_world.front_default}"/>
                    </div>
                    <div class="card-footer border-0" id="poke-footer-${index}" style="background: linear-gradient(90deg, ${gradient[0].color}, ${gradient[1].color})">
                        <h5 class="card-title pb-0 mb-0 text-capitalize d-flex justify-content-between align-items-center" style="color: ${gradient[0].textColor}">
                            ${res.name}
                            <a href="#!" onclick="pokeDetail('${data.url}', '${gradient[0].color}, ${gradient[1].color}', '${gradient[0].textColor}', '${gradient[1].textColor}')" data-bs-toggle="modal" data-bs-target="#poke-modal" style="font-size: 12px;" class="pt-2 ms-2"><i class="bi bi-arrow-up-right" style="color: ${gradient[1].textColor}"></i></a>
                        </h5>
                    </div>
                </div>
            </div>`

                $("#poke-list").append(poke)
                $("#pages").removeClass("d-none")
                removeLoading()

                /*console.log(res.sprites.other.dream_world.front_default);
    
                let active = "";
                if (index == 0) {
                    active = " active"
                }
                let carousel_inner = `<div class="carousel-item${active}">
                            <img class="d-block w-100" src="${res.sprites.other.dream_world.front_default}" alt="${res.name}">
                        </div>`;
    
                let carousel_indicators = `<li data-target="#carouselExampleIndicators" data-slide-to="${index}" class="${active}"></li>`
    
                $(".carousel-inner").append(carousel_inner)
                $(".carousel-indicators").append(carousel_indicators)*/
            });
        });
    });
}


function pokeDetail(url, headerBackground, textColor, closeColor) {
    loading('#poke-detail .modal-body')

    $.ajax({
        url: url
    }).done(res => {
        //console.log(res);

        let poke = `<div class="modal-header" style="background: linear-gradient(90deg, ${headerBackground})">
                <h5 class="modal-title text-capitalize" style="color: ${textColor}">${res.name}</h5>
                <button type="button" class="btn btn-sm btn-light text-dark" data-bs-dismiss="modal" aria-label="Close">
                <i class="bi bi-x-lg"></i> Close</button>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-5 text-center">
                    <div id="carouselExampleIndicators" class="carousel slide">
  <div class="d-none carousel-indicators">
    <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
    <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="1" aria-label="Slide 2"></button>
    <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="2" aria-label="Slide 3"></button>
  </div>
  <div class="carousel-inner">
    <div class="carousel-item active">
      <img src="${res.sprites.other.dream_world.front_default}" class="d-block poke w-100" alt="...">
    </div>
    <div class="carousel-item">
      <img src="${res.sprites.other.home.front_default}" class="d-block poke w-100" alt="...">
    </div>
    <div class="carousel-item">
      <img src="${res.sprites.other.home.front_shiny}" class="d-block poke w-100" alt="...">
    </div>
  </div>
  <button class="carousel-control-prev bg-secondary" style="height: 50px; top: calc(50% - 50px)" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="prev">
    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Previous</span>
  </button>
  <button class="carousel-control-next bg-secondary" style="height: 50px; top: calc(50% - 50px)" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="next">
    <span class="carousel-control-next-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Next</span>
  </button>
</div>
                        <img class="poke d-none" src="${res.sprites.other.dream_world.front_default}"/>
                    </div>

                    <div class="col-md-7 pe-2">
                        <div class="accordion" id="accordion-poke">
                          <div class="accordion-item">
                            <h2 class="accordion-header">
                              <button class="accordion-button fw-bold" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                Abilities
                              </button>
                            </h2>
                            <div id="collapseOne" class="accordion-collapse collapse show" data-bs-parent="#accordion-poke">
                              <div class="accordion-body p-0">
                                <div class="panel px-0" id="ability">
                                    <ol class="list-group" id="abilities">
                              
                                    </ol>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div class="accordion-item">
                            <h2 class="accordion-header">
                              <button class="accordion-button fw-bold collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                Stats
                              </button>
                            </h2>
                            <div id="collapseTwo" class="accordion-collapse collapse" data-bs-parent="#accordion-poke">
                              <div class="accordion-body p-0">
                                <div class="panel p-3" id="stat">
                           
                                </div>
                              </div>
                            </div>
                          </div>
                       </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer d-flex justify-content-between">
                <button class="btn btn-sm border-0 text-secondary rounded-circle small"><i class="bi bi-info-circle"></i> Data from <a class="text-underline" href="${url}">${url}</a></button>
            </div>`;

        $("#poke-detail").html(poke);
        removeLoading()

        getAbilities(res.abilities)
        getStat(res.stats)
    });
}

function getAbilities(data) {
    loading("#abilities")
    let effects = [];

    function fetchAbilityEffect(url) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: url
            }).done(res => {
                const effectEntries = res.effect_entries;
                const englishEntry = effectEntries.find(entry => entry.language.name === "en");

                if (englishEntry) {
                    resolve(englishEntry.effect);
                } else {
                    // Find any language on first array
                    resolve(res.effect_entries[0].effect);
                }
            }).fail(error => {
                reject(error);
            });
        });
    }

    let promises = data.map(item => {
        let newEffect = {
            "name": item.ability.name,
            "effect": ""
        };

        return fetchAbilityEffect(item.ability.url)
            .then(effect => {
                newEffect.effect = effect;
                return newEffect;
            })
            .catch(error => {
                console.error('Error fetching ability effect:', error);
                return newEffect;
            });
    });

    Promise.all(promises)
        .then(results => {
            effects = results;

            for (let j = 0; j < effects.length; j++) {
                let divider = ""
                if (j != effects.length-1) {
                    divider = " border-bottom"
                }
                let abilities = `<li class="list-group-item d-flex justify-content-between align-items-start border-0${divider}">
                          <div class="me-auto">
                            <div class="fw-bold text-capitalize">${effects[j].name}</div>
                            <div class="small">${effects[j].effect}</div>
                          </div>
                        </li>`;

                $("#abilities").append(abilities);
            }
        });

    removeLoading()
}

function setTabActive(obj, target) {
    $('.nav-link').removeClass('active'); $(obj).addClass('active')

    $(".panel").addClass('d-none');
    $(target).removeClass('d-none');
}

function getStat(data) {
    loading("#stat")
    let bar_color = ['success', 'danger', 'primary', 'warning', 'info', 'dark'];
    for (let i = 0; i < data.length; i++) {
        let stat_item = `<h6 class="text-capitalize mb-1 mt-1">${data[i].stat.name}</h6>
    <div class="progress" role="progressbar" aria-valuenow="${data[i].base_stat}" aria-valuemin="0" aria-valuemax="100">
                              <div class="progress-bar bg-${bar_color[i]}" style="width: ${data[i].base_stat}%">${data[i].base_stat}%</div>
                            </div>`

        $("#stat").append(stat_item);
    }

    removeLoading()
}

function loading(target) {
    let loader = `<div class="text-center loading">
                    <div class="spinner-border text-danger text-center" role="status">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                </div>`
    $(target).html(loader)
}

function removeLoading() {
    $(".loading").remove()
}

function setFooterBackgroundColor(img, index) {
    var canvas = document.createElement("canvas");
    var ctx = canvas.getContext("2d");

    canvas.width = img.width;
    canvas.height = img.height;

    ctx.drawImage(img, 0, 0);

    var imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
    var data = imageData.data;

    // Calculate the dominant color
    var colorCount = {};
    var maxCount = 0;
    var dominantColor = null;
    for (var i = 0; i < data.length; i += 4) {
        var r = data[i];
        var g = data[i + 1];
        var b = data[i + 2];
        var rgb = r + "," + g + "," + b;

        if (!colorCount[rgb]) {
            colorCount[rgb] = 0;
        }

        colorCount[rgb]++;

        if (colorCount[rgb] > maxCount) {
            maxCount = colorCount[rgb];
            dominantColor = rgb;
        }
    }

    // Set the background color of the footer element
    var footer = document.getElementById("poke-footer-" + index);
    footer.style.backgroundColor = "rgb(" + dominantColor + ")";
}

function getRandomColor() {
    // Generate random RGB values within the range of 0-255
    var r = Math.floor(Math.random() * 256);
    var g = Math.floor(Math.random() * 256);
    var b = Math.floor(Math.random() * 256);

    // Return the RGB color string
    return "rgb(" + r + "," + g + "," + b + ")";
}

function calculateBrightness(color) {
    // Remove the "rgb(" and ")" parts from the color string
    var rgb = color.slice(4, -1);

    // Split the RGB values into an array
    var values = rgb.split(",").map(function (val) {
        return parseInt(val.trim());
    });

    // Calculate the brightness using the relative luminance formula
    var brightness = (values[0] * 299 + values[1] * 587 + values[2] * 114) / 1000;

    return brightness;
}
function setGradientBackground(numColors) {
    var gradientColors = [];

    // Generate random colors and add them to the gradientColors array
    for (var i = 0; i < numColors; i++) {
        gradientColors.push(getRandomColor());
    }

    // Determine text color for each gradient color
    var gradientWithText = gradientColors.map(function (color) {
        var brightness = calculateBrightness(color);
        var textColor = brightness > 128 ? "rgb(0,0,0)" : "rgb(255,255,255)";
        return { color: color, textColor: textColor };
    });

    // Return the array of gradient colors with text colors
    return gradientWithText;
}