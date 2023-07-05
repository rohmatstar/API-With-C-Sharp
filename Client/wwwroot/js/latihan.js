let paragraph = document.querySelector('p[data-target="p"]');
let show_hide_p_btn = document.querySelector('button[data-trigger="show-hide-p"]');
let expanded_image = document.querySelector('img[data-trigger="expand-image"]');
let focus_on_me = document.querySelector('input[data-trigger="focus-on-me"]');
let random_bg = document.querySelector('button[data-trigger="random-bg"]');

// Click
let visible = true;
show_hide_p_btn.addEventListener("click", () => {
    visible = !visible;
    if (visible) {
        paragraph.style.visibility = "visible";
        show_hide_p_btn.innerText = "Sembunyikan Paragraf";
    }
    else {
        paragraph.style.visibility = "hidden";
        show_hide_p_btn.innerText = "Tampilkan Paragraf";
    }
});

// Mouseover & Mouseleave
expanded_image.addEventListener("mouseover", () => {
    expanded_image.classList.add("expanded");
});
expanded_image.addEventListener("mouseleave", () => {
    expanded_image.classList.remove("expanded");
});

// Focus & Blur
focus_on_me.addEventListener("focus", () => {
    focus_on_me.value = "Yeah, That's Great!";
    focus_on_me.classList.remove("text-danger");
    focus_on_me.classList.remove("border-danger");
    focus_on_me.classList.add("text-success");
    focus_on_me.classList.add("border-success");
});

focus_on_me.addEventListener("blur", () => {
    focus_on_me.value = "Focus on Me!";
    focus_on_me.classList.remove("text-success");
    focus_on_me.classList.remove("text-success");
    focus_on_me.classList.add("border-danger");
    focus_on_me.classList.add("text-danger");
});

// Right Click
random_bg.addEventListener("contextmenu", (e) => {
    e.preventDefault();
    let randomColor = Math.floor(Math.random() * 16777215).toString(16);
    document.body.style.backgroundColor = "#" + randomColor;
});
random_bg.addEventListener("click", (e) => {
    e.preventDefault();
    document.body.style.backgroundColor = "#ffffff";
});