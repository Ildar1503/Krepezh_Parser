const searchBtn = document.getElementById('search_btn');

searchBtn.addEventListener('click', async (event) =>
{
    event.preventDefault();

    const form = document.getElementById('search_form');
    const request = form.querySelector('[name=request').value;
    const shop_name = form.querySelector('[name=shop_name]').value;

    await GetData(shop_name, request);
});

async function GetData(shop_name, request)
{
    const response = await fetch("/GetTable&shopName=" + shop_name + "&request=" + request,
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

