const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5230/auctionHub")
    .build();


connection.on("AuctionItemAdded", function(response) {
    const auctionItem = response.data ? response.data : response;
    console.log("AuctionItem:", auctionItem); 

    const list = document.getElementById("auctionList");
    const item = document.createElement("li");
    const start = auctionItem.startTime ? new Date(auctionItem.startTime).toLocaleString() : "N/A";
    const end = auctionItem.endTime ? new Date(auctionItem.endTime).toLocaleString() : "N/A";
    const currentBid = auctionItem.currentBid ? `$${auctionItem.currentBid}` : "No bids yet";

    let attachmentsHtml = "";
    if (auctionItem.files && auctionItem.files.length > 0) {
        attachmentsHtml = `<span class="auction-attachments">| Attachments: `;
        attachmentsHtml += auctionItem.files.map(att =>
            `<a href="${att.downloadUrl}" download="${att.name}" class="attachment-link">${att.name}</a>`
        ).join(", ");
        attachmentsHtml += `</span>`;
    }

    item.innerHTML = `
        <span class="auction-name">Auction: ${auctionItem.name}</span>
        <span class="auction-id">(ID: ${auctionItem.id})</span>
        <span class="auction-time">| Start: <span class="time">${start}</span> | End: <span class="time">${end}</span></span>
        <span class="auction-bid">| Current Bid: <span class="amount">${currentBid}</span></span>
        ${attachmentsHtml}
    `;
    item.setAttribute("data-highest-bid", auctionItem.currentBid || 0);
    item.setAttribute("data-id", auctionItem.id);
    list.appendChild(item);
});

connection.on("ReceiveBidUpdate", function(auctionItemId, bidAmount, bidderId) {
    const bidList = document.getElementById("bidList");
    const bidItem = document.createElement("li");
    bidItem.innerHTML = `
        Auction <span class="bid-auction-id">${auctionItemId}</span>:
        <span class="bid-amount">$${bidAmount}</span>
        by <span class="bid-bidder">${bidderId}</span>
    `;
    bidList.appendChild(bidItem);

    const auctionList = document.getElementById("auctionList");
    const auctionLi = Array.from(auctionList.children).find(li =>
        li.getAttribute("data-id") === auctionItemId
    );
    if (auctionLi) {
        auctionLi.setAttribute("data-highest-bid", bidAmount);
        const amountSpan = auctionLi.querySelector(".amount");
        if (amountSpan) {
            amountSpan.textContent = `$${bidAmount}`;
        }
    }
});
connection.start().catch(err => console.error(err));

document.getElementById("bidBtn").addEventListener("click", function(e) {
    e.preventDefault();
    const auctionItemId = document.getElementById("auctionItemId").value;
    const bidAmount = parseFloat(document.getElementById("bidAmount").value);
    const bidderId = document.getElementById("bidderId").value;
    const errorDiv = document.getElementById("bidError");
    errorDiv.textContent = "";

    const auctionList = document.getElementById("auctionList");
    const auctionExists = Array.from(auctionList.children).some(li =>
        li.textContent.includes(auctionItemId)
    );

    if (!auctionItemId) {
        errorDiv.textContent = "Auction ID is required.";
        return;
    }
    if (!auctionExists) {
        errorDiv.textContent = "Auction ID not found.";
        return;
    }
    if (!bidderId) {
        errorDiv.textContent = "Bidder ID is required.";
        return;
    }
    if (!bidAmount || bidAmount <= 0) {
        errorDiv.textContent = "Bid amount must be greater than 0.";
        return;
    }

    connection.invoke("SendBidUpdate", auctionItemId, bidAmount, bidderId)
        .catch(err => {
            errorDiv.textContent = "Error sending bid: " + err;
            console.error(err);
        });
});