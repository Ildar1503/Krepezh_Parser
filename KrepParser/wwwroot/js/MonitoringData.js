let isSelected = false;

const form = document.getElementById('search_form');
const requestText = document.getElementById('request_text');
const categorySelect = document.getElementById('category_select');
const informationBtn = document.getElementById('information_btn');

requestText.addEventListener('change', TextOnChange);
categorySelect.addEventListener('change', SelectOnChange);

informationBtn.addEventListener('click', async (event) => {
    event.preventDefault();

    let request = form.querySelector('[name=request').value;
    const shop_name = form.querySelector('[name=shop_name]').value;

    if (isSelected) {
        request = form.querySelector('[name=category]').value;
        console.log(request);
    } 

    await GetInformationData(shop_name, request);
});

function TextOnChange(e) {
    e.preventDefault();
    console.log(requestText.value);
    console.log("text");
    if (requestText.value.length == 0) {
        categorySelect.style.visibility = 'visible';
    }
    else {
        categorySelect.style.visibility = 'hidden';
        isSelected = false;
    }
}

function SelectOnChange(e) {
    e.preventDefault();
    isSelected = true;
    console.log(form.querySelector('[name=category]').value);
}

async function GetInformationData(shop_name, request) {
    const response = await fetch("/GetInformationTable&shopName=" + shop_name + "&request=" + request,
    {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });

    if (response.ok == true) {
        const data = await response.json();
        const table = document.querySelector('.table_box');
        table.innerHTML = data;
    }
}