// Array JS
//array of object
let arrayMhsObj = [
    { nama: "budi", nim: "a112015", umur: 20, isActive: true, fakultas: { name: "komputer" } },
    { nama: "joko", nim: "a112035", umur: 22, isActive: false, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112020", umur: 21, isActive: true, fakultas: { name: "komputer" } },
    { nama: "herul", nim: "a112032", umur: 25, isActive: true, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112040", umur: 21, isActive: true, fakultas: { name: "komputer" } },
]

// Soal Nomor 1
let fakultasKomputer = [];
let getFakultasKomputer = (arrayName) => {
    for (let i = 0; i < arrayName.length; i++) {
        if (arrayName[i].fakultas.name == "komputer") {
            fakultasKomputer.push(arrayName[i]);
        }
    }
    console.log(fakultasKomputer);
}
console.log("Output Soal Nomor 1");
getFakultasKomputer(arrayMhsObj);


// Soal Nomor 2
let validateIsActive = (arrayName) => {
    for (let i = 0; i < arrayName.length; i++) {
        if (parseInt(arrayName[i].nim.slice(-2)) >= 30) {
            arrayName[i].isActive = false;
        }
    }
    console.log(arrayName);
}

console.log("Output Soal Nomor 2");
validateIsActive(arrayMhsObj);