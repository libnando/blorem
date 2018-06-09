const connection = new signalR.HubConnectionBuilder()
    .withUrl("/mainHub")
    .build();

connection.on("ReceiveState", (state) => {
    const li = document.createElement("li");
    li.textContent = state;
    document.getElementById("statesList").appendChild(li);
});

connection.start().catch(err => console.error(err.toString()));

new BrMap({
    wrapper: '#br_mine', 
    selectStates: ['sc'],
    callbacks: {
        click: function (element, uf) { 
            connection.invoke("ChooseState", uf).catch(err => console.error(err.toString()));
        },
    }
});