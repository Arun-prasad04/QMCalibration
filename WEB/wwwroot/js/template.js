//scoreCalculation1
function averagevaluecalculation() {

    var num1 = parseFloat(document.getElementById("QCEquipmentOneMeasuredValues.Val1").value);
    var num2 = parseFloat(document.getElementById("QCEquipmentOneMeasuredValues.Val2").value);
    var num3 = parseFloat(document.getElementById("QCEquipmentOneMeasuredValues.Val3").value);
    var num4 = parseFloat(document.getElementById("QCEquipmentOneMeasuredValues.Val4").value);
    var num5 = parseFloat(document.getElementById("QCEquipmentOneMeasuredValues.Val5").value);

    var num6 = parseFloat(document.getElementById("QCEquipmentOneMeasuredValues.Val6").value);
    var num7 = parseFloat(document.getElementById("QCEquipmentOneMeasuredValues.Val7").value);
    var num8 = parseFloat(document.getElementById("QCEquipmentOneMeasuredValues.Val8").value);
    var num9 = parseFloat(document.getElementById("QCEquipmentOneMeasuredValues.Val9").value);
    var num10 = parseFloat(document.getElementById("QCEquipmentOneMeasuredValues.Val10").value);

    var num11 = parseFloat(document.getElementById("QCEquipmentTwoMeasuredValues.Val1").value);
    var num12 = parseFloat(document.getElementById("QCEquipmentTwoMeasuredValues.Val2").value);
    var num13 = parseFloat(document.getElementById("QCEquipmentTwoMeasuredValues.Val3").value);
    var num14 = parseFloat(document.getElementById("QCEquipmentTwoMeasuredValues.Val4").value);

    var num15 = parseFloat(document.getElementById("QCEquipmentTwoMeasuredValues.Val5").value);
    var num16 = parseFloat(document.getElementById("QCEquipmentTwoMeasuredValues.Val6").value);
    var num17 = parseFloat(document.getElementById("QCEquipmentTwoMeasuredValues.Val7").value);
    var num18 = parseFloat(document.getElementById("QCEquipmentTwoMeasuredValues.Val8").value);

    var num19 = parseFloat(document.getElementById("QCEquipmentTwoMeasuredValues.Val9").value);
    var num20 = parseFloat(document.getElementById("QCEquipmentTwoMeasuredValues.Val10").value);





    num1 = isNaN(num1) ? null : num1;
    num2 = isNaN(num2) ? null : num2;
    num3 = isNaN(num3) ? null : num3;
    num4 = isNaN(num4) ? null : num4;
    num5 = isNaN(num5) ? null : num5;
    num6 = isNaN(num6) ? null : num6;
    num7 = isNaN(num7) ? null : num7;
    num8 = isNaN(num8) ? null : num8;
    num9 = isNaN(num9) ? null : num9;
    num10 = isNaN(num10) ? null : num10;

    num11 = isNaN(num11) ? null : num11;
    num12 = isNaN(num12) ? null : num12;
    num13 = isNaN(num13) ? null : num13;
    num14 = isNaN(num14) ? null : num14;
    num15 = isNaN(num15) ? null : num15;
    num16 = isNaN(num16) ? null : num16;
    num17 = isNaN(num17) ? null : num17;
    num18 = isNaN(num18) ? null : num18;
    num19 = isNaN(num19) ? null : num19;
    num20 = isNaN(num20) ? null : num20;


    count1 = 10;
    arrayvalues1 = [num1, num2, num3, num4, num5, num6, num7, num8, num9, num10];
    for (let i = 0; i < arrayvalues1.length; i++) {
        if (arrayvalues1[i] == null || arrayvalues1[i] == 0) {
            count1 = count1 - 1;
        }
    }

    count2 = 10;
    arrayvalues2 = [num11, num12, num13, num14, num15, num16, num17, num18, num19, num20];
    for (let j = 0; j < arrayvalues2.length; j++) {
        if (arrayvalues2[j] == null || arrayvalues2[j] == 0) {
            count2 = count2 - 1;
        }
    }
    sumofarray1 = (num1 + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9 + num10);
    sumofarray2 = (num11 + num12 + num13 + num14 + num15 + num16 + num17 + num18 + num19 + num20);


    var averagevalue1 = getaverage(sumofarray1, count1);
    var averagevalue2 = getaverage(sumofarray2, count2);


    if (count1 == 0) {
        document.getElementById("Mux1AvgValue").value = 0;

    }
    else {
        document.getElementById("Mux1AvgValue").value = averagevalue1;
    }
    if (count2 == 0) {
        document.getElementById("Mux2AvgValue").value = 0;

    }
    else {
        document.getElementById("Mux2AvgValue").value = averagevalue2;
    }

    //Get Squre value and EnValue based on this function
    squarecalculation();

}



function squarecalculation() {
    var sqr1 = parseFloat(document.getElementById("MesuredValuexOne").value);
    var sqr2 = parseFloat(document.getElementById("MesuredValueXTwo").value);

    var sqr3 = parseFloat(document.getElementById("Mux1SqrValue").value);
    var sqr4 = parseFloat(document.getElementById("Mux2SqrValue").value);

    var x = parseFloat(document.getElementById("Mux1AvgValue").value);
    var X = parseFloat(document.getElementById("Mux2AvgValue").value);


    sqr1 = isNaN(sqr1) ? 0 : sqr1;
    sqr2 = isNaN(sqr2) ? 0 : sqr2;
    sqr3 = isNaN(sqr3) ? 0 : sqr3;
    sqr4 = isNaN(sqr4) ? 0 : sqr4;
    x = isNaN(x) ? 0 : x;
    X = isNaN(X) ? 0 : X;


    var U1 = getsquare(sqr1);
    var U2 = getsquare(sqr2);

    document.getElementById("Mux1SqrValue").value = U1.toFixed(6);
    document.getElementById("Mux2SqrValue").value = U2.toFixed(6);

    var Envalue = getEnvalue(x, X, U1, U2);

    document.getElementById("EnValue").value = Envalue;



}
// js for alternative calibration Ended

// Generic Functions for Squarecalculation,AverageCalculation,EnvalueCalculation Started
function getsquare(square) {
    square = isNaN(square) ? 0 : square;
    Square = square * square;
    return Square;

}
function getaverage(sum, avg) {
    sum = isNaN(sum) ? 0 : sum;
    avg = isNaN(avg) ? 0 : avg;
    Average = sum / avg;
    Average = isNaN(Average) ? 0 : Average;
    return Average.toFixed(6);
}
function getEnvalue(x, X, U1, U2) {
    x = isNaN(x) ? 0 : x;
    X = isNaN(X) ? 0 : X;
    U1 = isNaN(U1) ? 0 : U1;
    U2 = isNaN(U2) ? 0 : U2;
    EnvalueCalculation = (x - X) / ((U1 + U2) ** 0.5);
    return EnvalueCalculation.toFixed(6);
}
// Generic Functions for Squarecalculation,AverageCalculation,EnvalueCalculation Ended

function averagevaluecalculation_Replicate() {

    var num1 = parseFloat(document.getElementById("Obs1.Observation1").value);
    var num2 = parseFloat(document.getElementById("Obs1.Observation2").value);
    var num3 = parseFloat(document.getElementById("Obs1.Observation3").value);
    var num4 = parseFloat(document.getElementById("Obs1.Observation4").value);
    var num5 = parseFloat(document.getElementById("Obs1.Observation5").value);

    var num6 = parseFloat(document.getElementById("Obs1.Observation6").value);
    var num7 = parseFloat(document.getElementById("Obs1.Observation7").value);
    var num8 = parseFloat(document.getElementById("Obs1.Observation8").value);
    var num9 = parseFloat(document.getElementById("Obs1.Observation9").value);
    var num10 = parseFloat(document.getElementById("Obs1.Observation10").value);
    
    var num11 = parseFloat(document.getElementById("Obs2.Observation1").value);
    var num12 = parseFloat(document.getElementById("Obs2.Observation2").value);
    var num13 = parseFloat(document.getElementById("Obs2.Observation3").value);
    var num14 = parseFloat(document.getElementById("Obs2.Observation4").value);
    var num15 = parseFloat(document.getElementById("Obs2.Observation5").value);

    var num16 = parseFloat(document.getElementById("Obs2.Observation6").value);
    var num17 = parseFloat(document.getElementById("Obs2.Observation7").value);
    var num18 = parseFloat(document.getElementById("Obs2.Observation8").value);
    var num19 = parseFloat(document.getElementById("Obs2.Observation9").value);
    var num20 = parseFloat(document.getElementById("Obs2.Observation10").value);

    num1 = isNaN(num1) ? null : num1;
    num2 = isNaN(num2) ? null : num2;
    num3 = isNaN(num3) ? null : num3;
    num4 = isNaN(num4) ? null : num4;
    num5 = isNaN(num5) ? null : num5;

    num6 = isNaN(num6) ? null : num6;
    num7 = isNaN(num7) ? null : num7;
    num8 = isNaN(num8) ? null : num8;
    num9 = isNaN(num9) ? null : num9;
    num10 = isNaN(num10) ? null : num10;

    num11 = isNaN(num11) ? null : num11;
    num12 = isNaN(num12) ? null : num12;
    num13 = isNaN(num13) ? null : num13;
    num14 = isNaN(num14) ? null : num14;
    num15 = isNaN(num15) ? null : num15;

    num16 = isNaN(num16) ? null : num16;
    num17 = isNaN(num17) ? null : num17;
    num18 = isNaN(num18) ? null : num18;
    num19 = isNaN(num19) ? null : num19;
    num20 = isNaN(num20) ? null : num20;


    count1 = 10;
    arrayvalues1 = [num1, num2, num3, num4, num5,num6,num7,num8,num9,num10];
    for (let i = 0; i < arrayvalues1.length; i++) {
        if (arrayvalues1[i] == null || arrayvalues1[i] == 0) {
            count1 = count1 - 1;
        }
    }

    count2 = 10;
    arrayvalues2 = [num11, num12, num13, num14, num15,num16,num17,num18,num19,num20];
    for (let j = 0; j < arrayvalues2.length; j++) {
        if (arrayvalues2[j] == null || arrayvalues2[j] == 0 ) {
            count2 = count2 - 1;
        }
    }
    sumofarray1 = (num1 + num2 + num3 + num4 + num5 +  num6 + num7 + num8 + num9 + num10);
    sumofarray2 = (num11 + num12 + num13 + num14 + num15 +  num16 + num17 + num18 + num19 + num20);

    var averagevalue1 = getaverage(sumofarray1, count1);
    var averagevalue2 = getaverage(sumofarray2, count2);


    if (count1 == 0) {
        document.getElementById("Obs1.Average").value = 0;

    }
    else {
        document.getElementById("Obs1.Average").value = averagevalue1;
    }
    if (count2 == 0) {
        document.getElementById("Obs2.Average").value = 0;

    }
    else {
        document.getElementById("Obs2.Average").value = averagevalue2;
    }

    setEnValue();

}

function squarecalculation_Replicate() {

    var sqr1 = parseFloat(document.getElementById("Obs1.MU").value);
    var sqr2 = parseFloat(document.getElementById("Obs2.MU").value);

    var x = parseFloat(document.getElementById("Obs1.Average").value);
    var X = parseFloat(document.getElementById("Obs2.Average").value);

    sqr1 = isNaN(sqr1) ? 0 : sqr1;
    sqr2 = isNaN(sqr2) ? 0 : sqr2;

    x = isNaN(x) ? 0 : x;
    X = isNaN(X) ? 0 : X;

    var U1 = getsquare(sqr1);
    var U2 = getsquare(sqr2);

    document.getElementById("Obs1.U2").value = U1.toFixed(7);
    document.getElementById("Obs2.U2").value = U2.toFixed(7);

    var Envalue = getEnvalue(x, X, U1, U2);

    document.getElementById("EnValue").value = Envalue;
}

function setEnValue(){

    var U1 = parseFloat(document.getElementById("Obs1.U2").value);
    var U2 = parseFloat(document.getElementById("Obs2.U2").value);

    var x = parseFloat(document.getElementById("Obs1.Average").value);
    var X = parseFloat(document.getElementById("Obs2.Average").value);

    U1 = isNaN(U1) ? 0 : U1;
    U2 = isNaN(U2) ? 0 : U2;
    
    x = isNaN(x) ? 0 : x;
    X = isNaN(X) ? 0 : X;

    var Envalue = getEnvalue(x, X, U1, U2);
    document.getElementById("EnValue").value = Envalue;
}

function averagemeaservalues() {

    var Actuals1_T_1 = parseFloat(document.getElementById("Actuals1_T_1").value);
    var Actuals1_T_2 = parseFloat(document.getElementById("Actuals1_T_2").value);
    var Actuals1_T_3 = parseFloat(document.getElementById("Actuals1_T_3").value);

    Actuals1_T_1 = isNaN(Actuals1_T_1) ? null : Actuals1_T_1;
    Actuals1_T_2 = isNaN(Actuals1_T_2) ? null : Actuals1_T_2;
    Actuals1_T_3 = isNaN(Actuals1_T_3) ? null : Actuals1_T_3;

    count1_1 = 3;
    arrayvalues1_1 = [Actuals1_T_1, Actuals1_T_2, Actuals1_T_3];
    for (let i = 0; i < arrayvalues1_1.length; i++) {
        if (arrayvalues1_1[i] == null || arrayvalues1_1[i] == 0) {
            count1_1 = count1_1 - 1;
        }
    }

    sumofarray1_1 = (Actuals1_T_1 + Actuals1_T_2 + Actuals1_T_3);

    var averagevalue1_1 = getaverage(sumofarray1_1, count1_1);

    document.getElementById("Avg1_1").value = averagevalue1_1;


    var Actuals1_T_4 = parseFloat(document.getElementById("Actuals1_T_4").value);
    var Actuals1_T_5 = parseFloat(document.getElementById("Actuals1_T_5").value);
    var Actuals1_T_6 = parseFloat(document.getElementById("Actuals1_T_6").value);

    Actuals1_T_4 = isNaN(Actuals1_T_4) ? null : Actuals1_T_4;
    Actuals1_T_5 = isNaN(Actuals1_T_5) ? null : Actuals1_T_5;
    Actuals1_T_6 = isNaN(Actuals1_T_6) ? null : Actuals1_T_6;

    count1_2 = 3;
    arrayvalues1_2 = [Actuals1_T_4, Actuals1_T_5, Actuals1_T_6];
    for (let i = 0; i < arrayvalues1_2.length; i++) {
        if (arrayvalues1_2[i] == null || arrayvalues1_2[i] == 0) {
            count1_2 = count1_2 - 1;
        }
    }

    sumofarray1_2 = (Actuals1_T_4 + Actuals1_T_5 + Actuals1_T_6);

    var averagevalue1_2 = getaverage(sumofarray1_2, count1_2);

    document.getElementById("Avg1_2").value = averagevalue1_2;



    var Actuals1_T_7 = parseFloat(document.getElementById("Actuals1_T_7").value);
    var Actuals1_T_8 = parseFloat(document.getElementById("Actuals1_T_8").value);
    var Actuals1_T_9 = parseFloat(document.getElementById("Actuals1_T_9").value);

    Actuals1_T_7 = isNaN(Actuals1_T_7) ? null : Actuals1_T_7;
    Actuals1_T_8 = isNaN(Actuals1_T_8) ? null : Actuals1_T_8;
    Actuals1_T_9 = isNaN(Actuals1_T_9) ? null : Actuals1_T_9;

    count1_3 = 3;
    arrayvalues1_3 = [Actuals1_T_7, Actuals1_T_8, Actuals1_T_9];
    for (let i = 0; i < arrayvalues1_3.length; i++) {
        if (arrayvalues1_3[i] == null || arrayvalues1_3[i] == 0) {
            count1_3 = count1_3 - 1;
        }
    }

    sumofarray1_3 = (Actuals1_T_7 + Actuals1_T_8 + Actuals1_T_9);

    var averagevalue1_3 = getaverage(sumofarray1_3, count1_3);

    document.getElementById("Avg1_3").value = averagevalue1_3;


    var Actuals1_T_10 = parseFloat(document.getElementById("Actuals1_T_10").value);
    var Actuals1_T_11 = parseFloat(document.getElementById("Actuals1_T_11").value);
    var Actuals1_T_12 = parseFloat(document.getElementById("Actuals1_T_12").value);

    Actuals1_T_10 = isNaN(Actuals1_T_10) ? null : Actuals1_T_10;
    Actuals1_T_11 = isNaN(Actuals1_T_11) ? null : Actuals1_T_11;
    Actuals1_T_12 = isNaN(Actuals1_T_12) ? null : Actuals1_T_12;

    count1_4 = 3;
    arrayvalues1_4 = [Actuals1_T_10, Actuals1_T_11, Actuals1_T_12];
    for (let i = 0; i < arrayvalues1_4.length; i++) {
        if (arrayvalues1_4[i] == null || arrayvalues1_4[i] == 0) {
            count1_4 = count1_4 - 1;
        }
    }

    sumofarray1_4 = (Actuals1_T_10 + Actuals1_T_11 + Actuals1_T_12);

    var averagevalue1_4 = getaverage(sumofarray1_4, count1_4);

    document.getElementById("Avg1_4").value = averagevalue1_4;


    var Actuals1_T_13 = parseFloat(document.getElementById("Actuals1_T_13").value);
    var Actuals1_T_14 = parseFloat(document.getElementById("Actuals1_T_14").value);
    var Actuals1_T_15 = parseFloat(document.getElementById("Actuals1_T_15").value);

    Actuals1_T_13 = isNaN(Actuals1_T_13) ? null : Actuals1_T_13;
    Actuals1_T_14 = isNaN(Actuals1_T_14) ? null : Actuals1_T_14;
    Actuals1_T_15 = isNaN(Actuals1_T_15) ? null : Actuals1_T_15;

    count1_5 = 3;
    arrayvalues1_5 = [Actuals1_T_13, Actuals1_T_14, Actuals1_T_15];
    for (let i = 0; i < arrayvalues1_5.length; i++) {
        if (arrayvalues1_5[i] == null || arrayvalues1_5[i] == 0) {
            count1_5 = count1_5 - 1;
        }
    }

    sumofarray1_5 = (Actuals1_T_13 + Actuals1_T_14 + Actuals1_T_15);

    var averagevalue1_5 = getaverage(sumofarray1_5, count1_5);

    document.getElementById("Avg1_5").value = averagevalue1_5;

    var Actuals2_T_1 = parseFloat(document.getElementById("Actuals2_T_1").value);
    var Actuals2_T_2 = parseFloat(document.getElementById("Actuals2_T_2").value);
    var Actuals2_T_3 = parseFloat(document.getElementById("Actuals2_T_3").value);

    Actuals2_T_1 = isNaN(Actuals2_T_1) ? null : Actuals2_T_1;
    Actuals2_T_2 = isNaN(Actuals2_T_2) ? null : Actuals2_T_2;
    Actuals2_T_3 = isNaN(Actuals2_T_3) ? null : Actuals2_T_3;

    count2_1 = 3;
    arrayvalues2_1 = [Actuals2_T_1, Actuals2_T_2, Actuals2_T_3];
    for (let i = 0; i < arrayvalues2_1.length; i++) {
        if (arrayvalues2_1[i] == null || arrayvalues2_1[i] == 0) {
            count2_1 = count2_1 - 1;
        }
    }

    sumofarray2_1 = (Actuals2_T_1 + Actuals2_T_2 + Actuals2_T_3);

    var averagevalue2_1 = getaverage(sumofarray2_1, count2_1);

    document.getElementById("Avg2_1").value = averagevalue2_1;


    var Actuals2_T_4 = parseFloat(document.getElementById("Actuals2_T_4").value);
    var Actuals2_T_5 = parseFloat(document.getElementById("Actuals2_T_5").value);
    var Actuals2_T_6 = parseFloat(document.getElementById("Actuals2_T_6").value);

    Actuals2_T_4 = isNaN(Actuals2_T_4) ? null : Actuals2_T_4;
    Actuals2_T_5 = isNaN(Actuals2_T_5) ? null : Actuals2_T_5;
    Actuals2_T_6 = isNaN(Actuals2_T_6) ? null : Actuals2_T_6;

    count2_2 = 3;
    arrayvalues2_2 = [Actuals2_T_4, Actuals2_T_5, Actuals2_T_6];
    for (let i = 0; i < arrayvalues2_2.length; i++) {
        if (arrayvalues2_2[i] == null || arrayvalues2_2[i] == 0) {
            count2_2 = count2_2 - 1;
        }
    }

    sumofarray2_2 = (Actuals2_T_4 + Actuals2_T_5 + Actuals2_T_6);

    var averagevalue2_2 = getaverage(sumofarray2_2, count2_2);

    document.getElementById("Avg2_2").value = averagevalue2_2;



    var Actuals2_T_7 = parseFloat(document.getElementById("Actuals2_T_7").value);
    var Actuals2_T_8 = parseFloat(document.getElementById("Actuals2_T_8").value);
    var Actuals2_T_9 = parseFloat(document.getElementById("Actuals2_T_9").value);

    Actuals2_T_7 = isNaN(Actuals2_T_7) ? null : Actuals2_T_7;
    Actuals2_T_8 = isNaN(Actuals2_T_8) ? null : Actuals2_T_8;
    Actuals2_T_9 = isNaN(Actuals2_T_9) ? null : Actuals2_T_9;

    count2_3 = 3;
    arrayvalues2_3 = [Actuals2_T_7, Actuals2_T_8, Actuals2_T_9];
    for (let i = 0; i < arrayvalues2_3.length; i++) {
        if (arrayvalues2_3[i] == null || arrayvalues2_3[i] == 0) {
            count2_3 = count2_3 - 1;
        }
    }

    sumofarray2_3 = (Actuals2_T_7 + Actuals2_T_8 + Actuals2_T_9);

    var averagevalue2_3 = getaverage(sumofarray2_3, count2_3);

    document.getElementById("Avg2_3").value = averagevalue2_3;


    var Actuals2_T_10 = parseFloat(document.getElementById("Actuals2_T_10").value);
    var Actuals2_T_11 = parseFloat(document.getElementById("Actuals2_T_11").value);
    var Actuals2_T_12 = parseFloat(document.getElementById("Actuals2_T_12").value);

    Actuals2_T_10 = isNaN(Actuals2_T_10) ? null : Actuals2_T_10;
    Actuals2_T_11 = isNaN(Actuals2_T_11) ? null : Actuals2_T_11;
    Actuals2_T_12 = isNaN(Actuals2_T_12) ? null : Actuals2_T_12;

    count2_4 = 3;
    arrayvalues2_4 = [Actuals2_T_10, Actuals2_T_11, Actuals2_T_12];
    for (let i = 0; i < arrayvalues2_4.length; i++) {
        if (arrayvalues2_4[i] == null || arrayvalues2_4[i] == 0) {
            count2_4 = count2_4 - 1;
        }
    }

    sumofarray2_4 = (Actuals2_T_10 + Actuals2_T_11 + Actuals2_T_12);

    var averagevalue2_4 = getaverage(sumofarray2_4, count2_4);

    document.getElementById("Avg2_4").value = averagevalue2_4;


    var Actuals2_T_13 = parseFloat(document.getElementById("Actuals2_T_13").value);
    var Actuals2_T_14 = parseFloat(document.getElementById("Actuals2_T_14").value);
    var Actuals2_T_15 = parseFloat(document.getElementById("Actuals2_T_15").value);

    Actuals2_T_13 = isNaN(Actuals2_T_13) ? null : Actuals2_T_13;
    Actuals2_T_14 = isNaN(Actuals2_T_14) ? null : Actuals2_T_14;
    Actuals2_T_15 = isNaN(Actuals2_T_15) ? null : Actuals2_T_15;

    count2_5 = 3;
    arrayvalues2_5 = [Actuals2_T_13, Actuals2_T_14, Actuals2_T_15];
    for (let i = 0; i < arrayvalues2_5.length; i++) {
        if (arrayvalues2_5[i] == null || arrayvalues2_5[i] == 0) {
            count2_5 = count2_5 - 1;
        }
    }

    sumofarray2_5 = (Actuals2_T_13 + Actuals2_T_14 + Actuals2_T_15);

    var averagevalue2_5 = getaverage(sumofarray2_5, count2_5);

    document.getElementById("Avg2_5").value = averagevalue2_5;

    var Actuals3_T_1 = parseFloat(document.getElementById("Actuals3_T_1").value);
    var Actuals3_T_2 = parseFloat(document.getElementById("Actuals3_T_2").value);
    var Actuals3_T_3 = parseFloat(document.getElementById("Actuals3_T_3").value);

    Actuals3_T_1 = isNaN(Actuals3_T_1) ? null : Actuals3_T_1;
    Actuals3_T_2 = isNaN(Actuals3_T_2) ? null : Actuals3_T_2;
    Actuals3_T_3 = isNaN(Actuals3_T_3) ? null : Actuals3_T_3;

    count3_1 = 3;
    arrayvalues3_1 = [Actuals3_T_1, Actuals3_T_2, Actuals3_T_3];
    for (let i = 0; i < arrayvalues3_1.length; i++) {
        if (arrayvalues3_1[i] == null || arrayvalues3_1[i] == 0) {
            count3_1 = count3_1 - 1;
        }
    }

    sumofarray3_1 = (Actuals3_T_1 + Actuals3_T_2 + Actuals3_T_3);

    var averagevalue3_1 = getaverage(sumofarray3_1, count3_1);

    document.getElementById("Avg3_1").value = averagevalue3_1;


    var Actuals3_T_4 = parseFloat(document.getElementById("Actuals3_T_4").value);
    var Actuals3_T_5 = parseFloat(document.getElementById("Actuals3_T_5").value);
    var Actuals3_T_6 = parseFloat(document.getElementById("Actuals3_T_6").value);

    Actuals3_T_4 = isNaN(Actuals3_T_4) ? null : Actuals3_T_4;
    Actuals3_T_5 = isNaN(Actuals3_T_5) ? null : Actuals3_T_5;
    Actuals3_T_6 = isNaN(Actuals3_T_6) ? null : Actuals3_T_6;

    count3_2 = 3;
    arrayvalues3_2 = [Actuals3_T_4, Actuals3_T_5, Actuals3_T_6];
    for (let i = 0; i < arrayvalues3_2.length; i++) {
        if (arrayvalues3_2[i] == null || arrayvalues3_2[i] == 0) {
            count3_2 = count3_2 - 1;
        }
    }

    sumofarray3_2 = (Actuals3_T_4 + Actuals3_T_5 + Actuals3_T_6);

    var averagevalue3_2 = getaverage(sumofarray3_2, count3_2);

    document.getElementById("Avg3_2").value = averagevalue3_2;



    var Actuals3_T_7 = parseFloat(document.getElementById("Actuals3_T_7").value);
    var Actuals3_T_8 = parseFloat(document.getElementById("Actuals3_T_8").value);
    var Actuals3_T_9 = parseFloat(document.getElementById("Actuals3_T_9").value);

    Actuals3_T_7 = isNaN(Actuals3_T_7) ? null : Actuals3_T_7;
    Actuals3_T_8 = isNaN(Actuals3_T_8) ? null : Actuals3_T_8;
    Actuals3_T_9 = isNaN(Actuals3_T_9) ? null : Actuals3_T_9;

    count3_3 = 3;
    arrayvalues3_3 = [Actuals3_T_7, Actuals3_T_8, Actuals3_T_9];
    for (let i = 0; i < arrayvalues3_3.length; i++) {
        if (arrayvalues3_3[i] == null || arrayvalues3_3[i] == 0) {
            count3_3 = count3_3 - 1;
        }
    }

    sumofarray3_3 = (Actuals3_T_7 + Actuals3_T_8 + Actuals3_T_9);

    var averagevalue3_3 = getaverage(sumofarray3_3, count3_3);

    document.getElementById("Avg3_3").value = averagevalue3_3;


    var Actuals3_T_10 = parseFloat(document.getElementById("Actuals3_T_10").value);
    var Actuals3_T_11 = parseFloat(document.getElementById("Actuals3_T_11").value);
    var Actuals3_T_12 = parseFloat(document.getElementById("Actuals3_T_12").value);

    Actuals3_T_10 = isNaN(Actuals3_T_10) ? null : Actuals3_T_10;
    Actuals3_T_11 = isNaN(Actuals3_T_11) ? null : Actuals3_T_11;
    Actuals3_T_12 = isNaN(Actuals3_T_12) ? null : Actuals3_T_12;

    count3_4 = 3;
    arrayvalues3_4 = [Actuals3_T_10, Actuals3_T_11, Actuals3_T_12];
    for (let i = 0; i < arrayvalues3_4.length; i++) {
        if (arrayvalues3_4[i] == null || arrayvalues3_4[i] == 0) {
            count3_4 = count3_4 - 1;
        }
    }

    sumofarray3_4 = (Actuals3_T_10 + Actuals3_T_11 + Actuals3_T_12);

    var averagevalue3_4 = getaverage(sumofarray3_4, count3_4);

    document.getElementById("Avg3_4").value = averagevalue3_4;


    var Actuals3_T_13 = parseFloat(document.getElementById("Actuals3_T_13").value);
    var Actuals3_T_14 = parseFloat(document.getElementById("Actuals3_T_14").value);
    var Actuals3_T_15 = parseFloat(document.getElementById("Actuals3_T_15").value);

    Actuals3_T_13 = isNaN(Actuals3_T_13) ? null : Actuals3_T_13;
    Actuals3_T_14 = isNaN(Actuals3_T_14) ? null : Actuals3_T_14;
    Actuals3_T_15 = isNaN(Actuals3_T_15) ? null : Actuals3_T_15;

    count3_5 = 3;
    arrayvalues3_5 = [Actuals3_T_13, Actuals3_T_14, Actuals3_T_15];
    for (let i = 0; i < arrayvalues3_5.length; i++) {
        if (arrayvalues3_5[i] == null || arrayvalues3_5[i] == 0) {
            count3_5 = count3_5 - 1;
        }
    }

    sumofarray3_5 = (Actuals3_T_13 + Actuals3_T_14 + Actuals3_T_15);

    var averagevalue3_5 = getaverage(sumofarray3_5, count3_5);

    document.getElementById("Avg3_5").value = averagevalue3_5;

    var Actuals4_T_1 = parseFloat(document.getElementById("Actuals4_T_1").value);
    var Actuals4_T_2 = parseFloat(document.getElementById("Actuals4_T_2").value);
    var Actuals4_T_3 = parseFloat(document.getElementById("Actuals4_T_3").value);

    Actuals4_T_1 = isNaN(Actuals4_T_1) ? null : Actuals4_T_1;
    Actuals4_T_2 = isNaN(Actuals4_T_2) ? null : Actuals4_T_2;
    Actuals4_T_3 = isNaN(Actuals4_T_3) ? null : Actuals4_T_3;

    count4_1 = 3;
    arrayvalues4_1 = [Actuals4_T_1, Actuals4_T_2, Actuals4_T_3];
    for (let i = 0; i < arrayvalues4_1.length; i++) {
        if (arrayvalues4_1[i] == null || arrayvalues4_1[i] == 0) {
            count4_1 = count4_1 - 1;
        }
    }

    sumofarray4_1 = (Actuals4_T_1 + Actuals4_T_2 + Actuals4_T_3);

    var averagevalue4_1 = getaverage(sumofarray4_1, count4_1);

    document.getElementById("Avg4_1").value = averagevalue4_1;


    var Actuals4_T_4 = parseFloat(document.getElementById("Actuals4_T_4").value);
    var Actuals4_T_5 = parseFloat(document.getElementById("Actuals4_T_5").value);
    var Actuals4_T_6 = parseFloat(document.getElementById("Actuals4_T_6").value);

    Actuals4_T_4 = isNaN(Actuals4_T_4) ? null : Actuals4_T_4;
    Actuals4_T_5 = isNaN(Actuals4_T_5) ? null : Actuals4_T_5;
    Actuals4_T_6 = isNaN(Actuals4_T_6) ? null : Actuals4_T_6;

    count4_2 = 3;
    arrayvalues4_2 = [Actuals4_T_4, Actuals4_T_5, Actuals4_T_6];
    for (let i = 0; i < arrayvalues4_2.length; i++) {
        if (arrayvalues4_2[i] == null || arrayvalues4_2[i] == 0) {
            count4_2 = count4_2 - 1;
        }
    }

    sumofarray4_2 = (Actuals4_T_4 + Actuals4_T_5 + Actuals4_T_6);

    var averagevalue4_2 = getaverage(sumofarray4_2, count4_2);

    document.getElementById("Avg4_2").value = averagevalue4_2;



    var Actuals4_T_7 = parseFloat(document.getElementById("Actuals4_T_7").value);
    var Actuals4_T_8 = parseFloat(document.getElementById("Actuals4_T_8").value);
    var Actuals4_T_9 = parseFloat(document.getElementById("Actuals4_T_9").value);

    Actuals4_T_7 = isNaN(Actuals4_T_7) ? null : Actuals4_T_7;
    Actuals4_T_8 = isNaN(Actuals4_T_8) ? null : Actuals4_T_8;
    Actuals4_T_9 = isNaN(Actuals4_T_9) ? null : Actuals4_T_9;

    count4_3 = 3;
    arrayvalues4_3 = [Actuals4_T_7, Actuals4_T_8, Actuals4_T_9];
    for (let i = 0; i < arrayvalues4_3.length; i++) {
        if (arrayvalues4_3[i] == null || arrayvalues4_3[i] == 0) {
            count4_3 = count4_3 - 1;
        }
    }

    sumofarray4_3 = (Actuals4_T_7 + Actuals4_T_8 + Actuals4_T_9);

    var averagevalue4_3 = getaverage(sumofarray4_3, count4_3);

    document.getElementById("Avg4_3").value = averagevalue4_3;


    var Actuals4_T_10 = parseFloat(document.getElementById("Actuals4_T_10").value);
    var Actuals4_T_11 = parseFloat(document.getElementById("Actuals4_T_11").value);
    var Actuals4_T_12 = parseFloat(document.getElementById("Actuals4_T_12").value);

    Actuals4_T_10 = isNaN(Actuals4_T_10) ? null : Actuals4_T_10;
    Actuals4_T_11 = isNaN(Actuals4_T_11) ? null : Actuals4_T_11;
    Actuals4_T_12 = isNaN(Actuals4_T_12) ? null : Actuals4_T_12;

    count4_4 = 3;
    arrayvalues4_4 = [Actuals4_T_10, Actuals4_T_11, Actuals4_T_12];
    for (let i = 0; i < arrayvalues4_4.length; i++) {
        if (arrayvalues4_4[i] == null || arrayvalues4_4[i] == 0) {
            count4_4 = count4_4 - 1;
        }
    }

    sumofarray4_4 = (Actuals4_T_10 + Actuals4_T_11 + Actuals4_T_12);

    var averagevalue4_4 = getaverage(sumofarray4_4, count4_4);

    document.getElementById("Avg4_4").value = averagevalue4_4;


    var Actuals4_T_13 = parseFloat(document.getElementById("Actuals4_T_13").value);
    var Actuals4_T_14 = parseFloat(document.getElementById("Actuals4_T_14").value);
    var Actuals4_T_15 = parseFloat(document.getElementById("Actuals4_T_15").value);

    Actuals4_T_13 = isNaN(Actuals4_T_13) ? null : Actuals4_T_13;
    Actuals4_T_14 = isNaN(Actuals4_T_14) ? null : Actuals4_T_14;
    Actuals4_T_15 = isNaN(Actuals4_T_15) ? null : Actuals4_T_15;

    count4_5 = 3;
    arrayvalues4_5 = [Actuals4_T_13, Actuals4_T_14, Actuals4_T_15];
    for (let i = 0; i < arrayvalues4_5.length; i++) {
        if (arrayvalues4_5[i] == null || arrayvalues4_5[i] == 0) {
            count4_5 = count4_5 - 1;
        }
    }

    sumofarray4_5 = (Actuals4_T_13 + Actuals4_T_14 + Actuals4_T_15);

    var averagevalue4_5 = getaverage(sumofarray4_5, count4_5);

    document.getElementById("Avg4_5").value = averagevalue4_5;

    var Actuals5_T_1 = parseFloat(document.getElementById("Actuals5_T_1").value);
    var Actuals5_T_2 = parseFloat(document.getElementById("Actuals5_T_2").value);
    var Actuals5_T_3 = parseFloat(document.getElementById("Actuals5_T_3").value);

    Actuals5_T_1 = isNaN(Actuals5_T_1) ? null : Actuals5_T_1;
    Actuals5_T_2 = isNaN(Actuals5_T_2) ? null : Actuals5_T_2;
    Actuals5_T_3 = isNaN(Actuals5_T_3) ? null : Actuals5_T_3;

    count5_1 = 3;
    arrayvalues5_1 = [Actuals5_T_1, Actuals5_T_2, Actuals5_T_3];
    for (let i = 0; i < arrayvalues5_1.length; i++) {
        if (arrayvalues5_1[i] == null || arrayvalues5_1[i] == 0) {
            count5_1 = count5_1 - 1;
        }
    }

    sumofarray5_1 = (Actuals5_T_1 + Actuals5_T_2 + Actuals5_T_3);

    var averagevalue5_1 = getaverage(sumofarray5_1, count5_1);

    document.getElementById("Avg5_1").value = averagevalue5_1;


    var Actuals5_T_4 = parseFloat(document.getElementById("Actuals5_T_4").value);
    var Actuals5_T_5 = parseFloat(document.getElementById("Actuals5_T_5").value);
    var Actuals5_T_6 = parseFloat(document.getElementById("Actuals5_T_6").value);

    Actuals5_T_4 = isNaN(Actuals5_T_4) ? null : Actuals5_T_4;
    Actuals5_T_5 = isNaN(Actuals5_T_5) ? null : Actuals5_T_5;
    Actuals5_T_6 = isNaN(Actuals5_T_6) ? null : Actuals5_T_6;

    count5_2 = 3;
    arrayvalues5_2 = [Actuals5_T_4, Actuals5_T_5, Actuals5_T_6];
    for (let i = 0; i < arrayvalues5_2.length; i++) {
        if (arrayvalues5_2[i] == null || arrayvalues5_2[i] == 0) {
            count5_2 = count5_2 - 1;
        }
    }

    sumofarray5_2 = (Actuals5_T_4 + Actuals5_T_5 + Actuals5_T_6);

    var averagevalue5_2 = getaverage(sumofarray5_2, count5_2);

    document.getElementById("Avg5_2").value = averagevalue5_2;



    var Actuals5_T_7 = parseFloat(document.getElementById("Actuals5_T_7").value);
    var Actuals5_T_8 = parseFloat(document.getElementById("Actuals5_T_8").value);
    var Actuals5_T_9 = parseFloat(document.getElementById("Actuals5_T_9").value);

    Actuals5_T_7 = isNaN(Actuals5_T_7) ? null : Actuals5_T_7;
    Actuals5_T_8 = isNaN(Actuals5_T_8) ? null : Actuals5_T_8;
    Actuals5_T_9 = isNaN(Actuals5_T_9) ? null : Actuals5_T_9;

    count5_3 = 3;
    arrayvalues5_3 = [Actuals5_T_7, Actuals5_T_8, Actuals5_T_9];
    for (let i = 0; i < arrayvalues5_3.length; i++) {
        if (arrayvalues5_3[i] == null || arrayvalues5_3[i] == 0) {
            count5_3 = count5_3 - 1;
        }
    }

    sumofarray5_3 = (Actuals5_T_7 + Actuals5_T_8 + Actuals5_T_9);

    var averagevalue5_3 = getaverage(sumofarray5_3, count5_3);

    document.getElementById("Avg5_3").value = averagevalue5_3;


    var Actuals5_T_10 = parseFloat(document.getElementById("Actuals5_T_10").value);
    var Actuals5_T_11 = parseFloat(document.getElementById("Actuals5_T_11").value);
    var Actuals5_T_12 = parseFloat(document.getElementById("Actuals5_T_12").value);

    Actuals5_T_10 = isNaN(Actuals5_T_10) ? null : Actuals5_T_10;
    Actuals5_T_11 = isNaN(Actuals5_T_11) ? null : Actuals5_T_11;
    Actuals5_T_12 = isNaN(Actuals5_T_12) ? null : Actuals5_T_12;

    count5_4 = 3;
    arrayvalues5_4 = [Actuals5_T_10, Actuals5_T_11, Actuals5_T_12];
    for (let i = 0; i < arrayvalues5_4.length; i++) {
        if (arrayvalues5_4[i] == null || arrayvalues5_4[i] == 0) {
            count5_4 = count5_4 - 1;
        }
    }

    sumofarray5_4 = (Actuals5_T_10 + Actuals5_T_11 + Actuals5_T_12);

    var averagevalue5_4 = getaverage(sumofarray5_4, count5_4);

    document.getElementById("Avg5_4").value = averagevalue5_4;


    var Actuals5_T_13 = parseFloat(document.getElementById("Actuals5_T_13").value);
    var Actuals5_T_14 = parseFloat(document.getElementById("Actuals5_T_14").value);
    var Actuals5_T_15 = parseFloat(document.getElementById("Actuals5_T_15").value);

    Actuals5_T_13 = isNaN(Actuals5_T_13) ? null : Actuals5_T_13;
    Actuals5_T_14 = isNaN(Actuals5_T_14) ? null : Actuals5_T_14;
    Actuals5_T_15 = isNaN(Actuals5_T_15) ? null : Actuals5_T_15;

    count5_5 = 3;
    arrayvalues5_5 = [Actuals5_T_13, Actuals5_T_14, Actuals5_T_15];
    for (let i = 0; i < arrayvalues5_5.length; i++) {
        if (arrayvalues5_5[i] == null || arrayvalues5_5[i] == 0) {
            count5_5 = count5_5 - 1;
        }
    }

    sumofarray5_5 = (Actuals5_T_13 + Actuals5_T_14 + Actuals5_T_15);

    var averagevalue5_5 = getaverage(sumofarray5_5, count5_5);

    document.getElementById("Avg5_5").value = averagevalue5_5;

    var Actuals6_T_1 = parseFloat(document.getElementById("Actuals6_T_1").value);
    var Actuals6_T_2 = parseFloat(document.getElementById("Actuals6_T_2").value);
    var Actuals6_T_3 = parseFloat(document.getElementById("Actuals6_T_3").value);

    Actuals6_T_1 = isNaN(Actuals6_T_1) ? null : Actuals6_T_1;
    Actuals6_T_2 = isNaN(Actuals6_T_2) ? null : Actuals6_T_2;
    Actuals6_T_3 = isNaN(Actuals6_T_3) ? null : Actuals6_T_3;

    count6_1 = 3;
    arrayvalues6_1 = [Actuals6_T_1, Actuals6_T_2, Actuals6_T_3];
    for (let i = 0; i < arrayvalues6_1.length; i++) {
        if (arrayvalues6_1[i] == null || arrayvalues6_1[i] == 0) {
            count6_1 = count6_1 - 1;
        }
    }

    sumofarray6_1 = (Actuals6_T_1 + Actuals6_T_2 + Actuals6_T_3);

    var averagevalue6_1 = getaverage(sumofarray6_1, count6_1);

    document.getElementById("Avg6_1").value = averagevalue6_1;


    var Actuals6_T_4 = parseFloat(document.getElementById("Actuals6_T_4").value);
    var Actuals6_T_5 = parseFloat(document.getElementById("Actuals6_T_5").value);
    var Actuals6_T_6 = parseFloat(document.getElementById("Actuals6_T_6").value);

    Actuals6_T_4 = isNaN(Actuals6_T_4) ? null : Actuals6_T_4;
    Actuals6_T_5 = isNaN(Actuals6_T_5) ? null : Actuals6_T_5;
    Actuals6_T_6 = isNaN(Actuals6_T_6) ? null : Actuals6_T_6;

    count6_2 = 3;
    arrayvalues6_2 = [Actuals6_T_4, Actuals6_T_5, Actuals6_T_6];
    for (let i = 0; i < arrayvalues6_2.length; i++) {
        if (arrayvalues6_2[i] == null || arrayvalues6_2[i] == 0) {
            count6_2 = count6_2 - 1;
        }
    }

    sumofarray6_2 = (Actuals6_T_4 + Actuals6_T_5 + Actuals6_T_6);

    var averagevalue6_2 = getaverage(sumofarray6_2, count6_2);

    document.getElementById("Avg6_2").value = averagevalue6_2;



    var Actuals6_T_7 = parseFloat(document.getElementById("Actuals6_T_7").value);
    var Actuals6_T_8 = parseFloat(document.getElementById("Actuals6_T_8").value);
    var Actuals6_T_9 = parseFloat(document.getElementById("Actuals6_T_9").value);

    Actuals6_T_7 = isNaN(Actuals6_T_7) ? null : Actuals6_T_7;
    Actuals6_T_8 = isNaN(Actuals6_T_8) ? null : Actuals6_T_8;
    Actuals6_T_9 = isNaN(Actuals6_T_9) ? null : Actuals6_T_9;

    count6_3 = 3;
    arrayvalues6_3 = [Actuals6_T_7, Actuals6_T_8, Actuals6_T_9];
    for (let i = 0; i < arrayvalues6_3.length; i++) {
        if (arrayvalues6_3[i] == null || arrayvalues6_3[i] == 0) {
            count6_3 = count6_3 - 1;
        }
    }

    sumofarray6_3 = (Actuals6_T_7 + Actuals6_T_8 + Actuals6_T_9);

    var averagevalue6_3 = getaverage(sumofarray6_3, count6_3);

    document.getElementById("Avg6_3").value = averagevalue6_3;


    var Actuals6_T_10 = parseFloat(document.getElementById("Actuals6_T_10").value);
    var Actuals6_T_11 = parseFloat(document.getElementById("Actuals6_T_11").value);
    var Actuals6_T_12 = parseFloat(document.getElementById("Actuals6_T_12").value);

    Actuals6_T_10 = isNaN(Actuals6_T_10) ? null : Actuals6_T_10;
    Actuals6_T_11 = isNaN(Actuals6_T_11) ? null : Actuals6_T_11;
    Actuals6_T_12 = isNaN(Actuals6_T_12) ? null : Actuals6_T_12;

    count6_4 = 3;
    arrayvalues6_4 = [Actuals6_T_10, Actuals6_T_11, Actuals6_T_12];
    for (let i = 0; i < arrayvalues6_4.length; i++) {
        if (arrayvalues6_4[i] == null || arrayvalues6_4[i] == 0) {
            count6_4 = count6_4 - 1;
        }
    }

    sumofarray6_4 = (Actuals6_T_10 + Actuals6_T_11 + Actuals6_T_12);

    var averagevalue6_4 = getaverage(sumofarray6_4, count6_4);

    document.getElementById("Avg6_4").value = averagevalue6_4;


    var Actuals6_T_13 = parseFloat(document.getElementById("Actuals6_T_13").value);
    var Actuals6_T_14 = parseFloat(document.getElementById("Actuals6_T_14").value);
    var Actuals6_T_15 = parseFloat(document.getElementById("Actuals6_T_15").value);

    Actuals6_T_13 = isNaN(Actuals6_T_13) ? null : Actuals6_T_13;
    Actuals6_T_14 = isNaN(Actuals6_T_14) ? null : Actuals6_T_14;
    Actuals6_T_15 = isNaN(Actuals6_T_15) ? null : Actuals6_T_15;

    count6_5 = 3;
    arrayvalues6_5 = [Actuals6_T_13, Actuals6_T_14, Actuals6_T_15];
    for (let i = 0; i < arrayvalues6_5.length; i++) {
        if (arrayvalues6_5[i] == null || arrayvalues6_5[i] == 0) {
            count6_5 = count6_5 - 1;
        }
    }

    sumofarray6_5 = (Actuals6_T_13 + Actuals6_T_14 + Actuals6_T_15);

    var averagevalue6_5 = getaverage(sumofarray6_5, count6_5);

    document.getElementById("Avg6_5").value = averagevalue6_5;
}

function getaverageForMicrometer(sum, avg) {
    sum = isNaN(sum) ? 0 : sum;
    avg = isNaN(avg) ? 0 : avg;
    Average = sum / avg;
    Average = isNaN(Average) ? 0 : Average;
    return Average.toFixed(3);
}

function averagemicrometervalues() {

    var ActualsT1_1 = parseFloat(document.getElementById("ActualsT11").value);
    var ActualsT2_1 = parseFloat(document.getElementById("ActualsT21").value);
    var ActualsT3_1 = parseFloat(document.getElementById("ActualsT31").value);

    ActualsT1_1 = isNaN(ActualsT1_1) ? null : ActualsT1_1;
    ActualsT2_1 = isNaN(ActualsT2_1) ? null : ActualsT2_1;
    ActualsT3_1 = isNaN(ActualsT3_1) ? null : ActualsT3_1;

    count1_1 = 3;
    arrayvalues1_1 = [ActualsT1_1, ActualsT2_1, ActualsT3_1];
    for (let i = 0; i < arrayvalues1_1.length; i++) {
        if (arrayvalues1_1[i] == null || arrayvalues1_1[i] == 0) {
            count1_1 = count1_1 - 1;
        }
    }

    sumofarray1_1 = (ActualsT1_1 + ActualsT2_1 + ActualsT3_1);

    var averagevalue1_1 = getaverageForMicrometer(sumofarray1_1, count1_1);

    document.getElementById("Avg1").value = averagevalue1_1;


    var ActualsT1_2 = parseFloat(document.getElementById("ActualsT12").value);
    var ActualsT2_2 = parseFloat(document.getElementById("ActualsT22").value);
    var ActualsT3_2 = parseFloat(document.getElementById("ActualsT32").value);

    ActualsT1_2 = isNaN(ActualsT1_2) ? null : ActualsT1_2;
    ActualsT2_2 = isNaN(ActualsT2_2) ? null : ActualsT2_2;
    ActualsT3_2 = isNaN(ActualsT3_2) ? null : ActualsT3_2;

    count1_2 = 3;
    arrayvalues1_2 = [ActualsT1_2, ActualsT2_2, ActualsT3_2];
    for (let i = 0; i < arrayvalues1_2.length; i++) {
        if (arrayvalues1_2[i] == null || arrayvalues1_2[i] == 0) {
            count1_2 = count1_2 - 1;
        }
    }

    sumofarray1_2 = (ActualsT1_2 + ActualsT2_2 + ActualsT3_2);

    var averagevalue1_2 = getaverageForMicrometer(sumofarray1_2, count1_2);

    document.getElementById("Avg2").value = averagevalue1_2;



    var ActualsT1_3 = parseFloat(document.getElementById("ActualsT13").value);
    var ActualsT2_3 = parseFloat(document.getElementById("ActualsT23").value);
    var ActualsT3_3 = parseFloat(document.getElementById("ActualsT33").value);

    ActualsT1_3 = isNaN(ActualsT1_3) ? null : ActualsT1_3;
    ActualsT2_3 = isNaN(ActualsT2_3) ? null : ActualsT2_3;
    ActualsT3_3 = isNaN(ActualsT3_3) ? null : ActualsT3_3;

    count1_3 = 3;
    arrayvalues1_3 = [ActualsT1_3, ActualsT2_3, ActualsT3_3];
    for (let i = 0; i < arrayvalues1_3.length; i++) {
        if (arrayvalues1_3[i] == null || arrayvalues1_3[i] == 0) {
            count1_3 = count1_3 - 1;
        }
    }

    sumofarray1_3 = (ActualsT1_3 + ActualsT2_3 + ActualsT3_3);

    var averagevalue1_3 = getaverageForMicrometer(sumofarray1_3, count1_3);

    document.getElementById("Avg3").value = averagevalue1_3;



    var ActualsT1_4 = parseFloat(document.getElementById("ActualsT14").value);
    var ActualsT2_4 = parseFloat(document.getElementById("ActualsT24").value);
    var ActualsT3_4 = parseFloat(document.getElementById("ActualsT34").value);

    ActualsT1_4 = isNaN(ActualsT1_4) ? null : ActualsT1_4;
    ActualsT2_4 = isNaN(ActualsT2_4) ? null : ActualsT2_4;
    ActualsT3_4 = isNaN(ActualsT3_4) ? null : ActualsT3_4;

    count1_4 = 3;
    arrayvalues1_4 = [ActualsT1_4, ActualsT2_4, ActualsT3_4];
    for (let i = 0; i < arrayvalues1_4.length; i++) {
        if (arrayvalues1_4[i] == null || arrayvalues1_4[i] == 0) {
            count1_4 = count1_4 - 1;
        }
    }

    sumofarray1_4 = (ActualsT1_4 + ActualsT2_4 + ActualsT3_4);

    var averagevalue1_4 = getaverageForMicrometer(sumofarray1_4, count1_4);

    document.getElementById("Avg4").value = averagevalue1_4;



    var ActualsT1_5 = parseFloat(document.getElementById("ActualsT15").value);
    var ActualsT2_5 = parseFloat(document.getElementById("ActualsT25").value);
    var ActualsT3_5 = parseFloat(document.getElementById("ActualsT35").value);

    ActualsT1_5 = isNaN(ActualsT1_5) ? null : ActualsT1_5;
    ActualsT2_5 = isNaN(ActualsT2_5) ? null : ActualsT2_5;
    ActualsT3_5 = isNaN(ActualsT3_5) ? null : ActualsT3_5;

    count1_5 = 3;
    arrayvalues1_5 = [ActualsT1_5, ActualsT2_5, ActualsT3_5];
    for (let i = 0; i < arrayvalues1_5.length; i++) {
        if (arrayvalues1_5[i] == null || arrayvalues1_5[i] == 0) {
            count1_5 = count1_5 - 1;
        }
    }

    sumofarray1_5 = (ActualsT1_5 + ActualsT2_5 + ActualsT3_5);

    var averagevalue1_5 = getaverageForMicrometer(sumofarray1_5, count1_5);

    document.getElementById("Avg5").value = averagevalue1_5;



    var ActualsT1_6 = parseFloat(document.getElementById("ActualsT16").value);
    var ActualsT2_6 = parseFloat(document.getElementById("ActualsT26").value);
    var ActualsT3_6 = parseFloat(document.getElementById("ActualsT36").value);

    ActualsT1_6 = isNaN(ActualsT1_6) ? null : ActualsT1_6;
    ActualsT2_6 = isNaN(ActualsT2_6) ? null : ActualsT2_6;
    ActualsT3_6 = isNaN(ActualsT3_6) ? null : ActualsT3_6;

    count1_6 = 3;
    arrayvalues1_6 = [ActualsT1_6, ActualsT2_6, ActualsT3_6];
    for (let i = 0; i < arrayvalues1_6.length; i++) {
        if (arrayvalues1_6[i] == null || arrayvalues1_6[i] == 0) {
            count1_6 = count1_6 - 1;
        }
    }

    sumofarray1_6 = (ActualsT1_6 + ActualsT2_6 + ActualsT3_6);

    var averagevalue1_6 = getaverageForMicrometer(sumofarray1_6, count1_6);

    document.getElementById("Avg6").value = averagevalue1_6;



    var ActualsT1_7 = parseFloat(document.getElementById("ActualsT17").value);
    var ActualsT2_7 = parseFloat(document.getElementById("ActualsT27").value);
    var ActualsT3_7 = parseFloat(document.getElementById("ActualsT37").value);

    ActualsT1_7 = isNaN(ActualsT1_7) ? null : ActualsT1_7;
    ActualsT2_7 = isNaN(ActualsT2_7) ? null : ActualsT2_7;
    ActualsT3_7 = isNaN(ActualsT3_7) ? null : ActualsT3_7;

    count1_7 = 3;
    arrayvalues1_7 = [ActualsT1_7, ActualsT2_7, ActualsT3_7];
    for (let i = 0; i < arrayvalues1_7.length; i++) {
        if (arrayvalues1_7[i] == null || arrayvalues1_7[i] == 0) {
            count1_7 = count1_7 - 1;
        }
    }

    sumofarray1_7 = (ActualsT1_7 + ActualsT2_7 + ActualsT3_7);

    var averagevalue1_7 = getaverageForMicrometer(sumofarray1_7, count1_7);

    document.getElementById("Avg7").value = averagevalue1_7;



    var ActualsT1_8 = parseFloat(document.getElementById("ActualsT18").value);
    var ActualsT2_8 = parseFloat(document.getElementById("ActualsT28").value);
    var ActualsT3_8 = parseFloat(document.getElementById("ActualsT38").value);

    ActualsT1_8 = isNaN(ActualsT1_8) ? null : ActualsT1_8;
    ActualsT2_8 = isNaN(ActualsT2_8) ? null : ActualsT2_8;
    ActualsT3_8 = isNaN(ActualsT3_8) ? null : ActualsT3_8;

    count1_8 = 3;
    arrayvalues1_8 = [ActualsT1_8, ActualsT2_8, ActualsT3_8];
    for (let i = 0; i < arrayvalues1_8.length; i++) {
        if (arrayvalues1_8[i] == null || arrayvalues1_8[i] == 0) {
            count1_8 = count1_8 - 1;
        }
    }

    sumofarray1_8 = (ActualsT1_8 + ActualsT2_8 + ActualsT3_8);

    var averagevalue1_8 = getaverageForMicrometer(sumofarray1_8, count1_8);

    document.getElementById("Avg8").value = averagevalue1_8;



    var ActualsT1_9 = parseFloat(document.getElementById("ActualsT19").value);
    var ActualsT2_9 = parseFloat(document.getElementById("ActualsT29").value);
    var ActualsT3_9 = parseFloat(document.getElementById("ActualsT39").value);

    ActualsT1_9 = isNaN(ActualsT1_9) ? null : ActualsT1_9;
    ActualsT2_9 = isNaN(ActualsT2_9) ? null : ActualsT2_9;
    ActualsT3_9 = isNaN(ActualsT3_9) ? null : ActualsT3_9;

    count1_9 = 3;
    arrayvalues1_9 = [ActualsT1_9, ActualsT2_9, ActualsT3_9];
    for (let i = 0; i < arrayvalues1_9.length; i++) {
        if (arrayvalues1_9[i] == null || arrayvalues1_9[i] == 0) {
            count1_9 = count1_9 - 1;
        }
    }

    sumofarray1_9 = (ActualsT1_9 + ActualsT2_9 + ActualsT3_9);

    var averagevalue1_9 = getaverageForMicrometer(sumofarray1_9, count1_9);

    document.getElementById("Avg9").value = averagevalue1_9;



    var ActualsT1_10 = parseFloat(document.getElementById("ActualsT110").value);
    var ActualsT2_10 = parseFloat(document.getElementById("ActualsT210").value);
    var ActualsT3_10 = parseFloat(document.getElementById("ActualsT310").value);

    ActualsT1_10 = isNaN(ActualsT1_10) ? null : ActualsT1_10;
    ActualsT2_10 = isNaN(ActualsT2_10) ? null : ActualsT2_10;
    ActualsT3_10 = isNaN(ActualsT3_10) ? null : ActualsT3_10;

    count1_10 = 3;
    arrayvalues1_10 = [ActualsT1_10, ActualsT2_10, ActualsT3_10];
    for (let i = 0; i < arrayvalues1_10.length; i++) {
        if (arrayvalues1_10[i] == null || arrayvalues1_10[i] == 0) {
            count1_10 = count1_10 - 1;
        }
    }

    sumofarray1_10 = (ActualsT1_10 + ActualsT2_10 + ActualsT3_10);

    var averagevalue1_10 = getaverageForMicrometer(sumofarray1_10, count1_10);

    document.getElementById("Avg10").value = averagevalue1_10;



    var ActualsT1_11 = parseFloat(document.getElementById("ActualsT111").value);
    var ActualsT2_11 = parseFloat(document.getElementById("ActualsT211").value);
    var ActualsT3_11 = parseFloat(document.getElementById("ActualsT311").value);

    ActualsT1_11 = isNaN(ActualsT1_11) ? null : ActualsT1_11;
    ActualsT2_11 = isNaN(ActualsT2_11) ? null : ActualsT2_11;
    ActualsT3_11 = isNaN(ActualsT3_11) ? null : ActualsT3_11;

    count1_11 = 3;
    arrayvalues1_11 = [ActualsT1_11, ActualsT2_11, ActualsT3_11];
    for (let i = 0; i < arrayvalues1_11.length; i++) {
        if (arrayvalues1_11[i] == null || arrayvalues1_11[i] == 0) {
            count1_11 = count1_11 - 1;
        }
    }

    sumofarray1_11 = (ActualsT1_11 + ActualsT2_11 + ActualsT3_11);

    var averagevalue1_11 = getaverageForMicrometer(sumofarray1_11, count1_11);

    document.getElementById("Avg11").value = averagevalue1_11;

}