
var uri = 'api/gamestate/2284a0d1-5b9f-4ff1-ac9a-e0fae61e0c6c';
var playerWhite = {};
var playerBlack = {};
var turnWhite;
var gameId;
var pieces = [];
var coordPieceMap = {}; // coord, Piece

$(document).ready(function ()
{
    var jrequest = $.getJSON(uri, function (data) {
        paintGame(data);
    });
});

function paintGame(data)
{
    $.each(data, function (key, val) {
        if (key == '_board') {
            $.each(data._board.PieceCoordMap, function (coord, val) {
                coordPieceMap[coord] = val;
            });

        }
        else if (key == 'PlayerWhite') {
            playerWhite = val;
            console.log("val.name:", val.Name);
        }
        else if (key == 'PlayerBlack') {
            playerBlack = val;
        }
        else if (key == 'TurnWhite') {
            turnWhite = val;
        }
        else if (key == 'GameID') {
            gameId = val;
        }
    });

    printBoard(coordPieceMap);
    printBlackPlayerInfo(playerBlack, turnWhite);
    printWhitePlayerInfo(playerWhite, turnWhite);
}


function submitMove()
{
    var startCoord = $('#startCoord').val();
    var parsedStartCoord = `(${startCoord.substring(0, 3)})`;
    var endXCoord = Number(startCoord.charAt(4));
    var endYCoord = Number(startCoord.charAt(6));
    var id;
    if (coordPieceMap.hasOwnProperty(parsedStartCoord)) {
        id = coordPieceMap[parsedStartCoord];
        id = id.PieceId;
    }
    updateMove(id, endXCoord, endYCoord);
}

function updateMove(id, endXCoord, endYCoord)
{
    fetch(uri + '/pieces/' + id,
        {
            method: "PUT", headers: {
                'Content-Type': 'application/json'
            }, body: JSON.stringify({ x: endXCoord, y: endYCoord, playerName: turnWhite ? playerWhite.Name : playerBlack.Name })
        })
        .then(res => res.json())
        .then(gameState => paintGame(gameState))
        .catch(e => console.error(e));
}


function printBoard(coordPieceMap)
{
    $("#board").empty();
    var myTable = $("<table oncontextmenu=\"return false\"></table>").appendTo("#board");
    var topRow = $("<tr></tr>").appendTo(myTable);
    var cellElement = document.createElement('td');
    cellElement.innerHTML = "  ";
    cellElement.className = "greenSquare";
    topRow.append(cellElement);
    for (var row = 0; row < 8; row++) {

        var cellElement = document.createElement('td');
        cellElement.innerHTML = row;
        cellElement.className = "greenSquare";
        topRow.append(cellElement);



        var myRow = $("<tr></tr>").appendTo(myTable);
        for (var col = 0; col < 8; col++) {
            if (col == 0) {
                var cellElement = document.createElement('td');
                cellElement.innerHTML = row;
                cellElement.className = "greenSquare";
                myRow.append(cellElement);
            }
            var squareColor = "blackSquare"
            if ((col + row) % 2 == 0) {
                squareColor = "isWhite";
            }

            var coord = `(${col},${row})`;
            var piece = coordPieceMap[coord];

            // create td element
            var cellElement = document.createElement('td');
            cellElement.id = coord;
            cellElement.className = squareColor;
            myRow.append(cellElement);

            var pieceOrNull;

            // if there is a piece on the coord, create a piece element
            // and append to td of matching coord cell
            var pieceElement = document.createElement('div');

            if (piece != undefined) {

                pieceElement.id = piece.PieceId;
                pieceElement.className = piece.White ? "whitePiece" : "blackPiece";
                pieceOrNull = piece.FirstLetter;

            } else {
                pieceOrNull = "  ";
            }
            pieceElement.innerHTML = pieceOrNull;
            cellElement.appendChild(pieceElement);

        }
    }
}

function printWhitePlayerInfo(playerWhite, turnWhite)
{
    $("#playerWhiteInfo").empty();
    var infoElement = document.createElement('div');
    if (playerWhite.White && turnWhite) {
        infoElement.className = "activePlayer";
    }
    infoElement.innerHTML = `White-Player: ${playerWhite.Name}`
    $("#playerWhiteInfo").append(infoElement);
}

function printBlackPlayerInfo(playerBlack, turnWhite)
{
    $("#playerBlackInfo").empty();
    var infoElement = document.createElement('div');
    if (!playerBlack.White && !turnWhite) {
        infoElement.className = "activePlayer";
    }
    infoElement.innerHTML = `Black-Player: ${playerBlack.Name}`
    $("#playerBlackInfo").append(infoElement);
}
