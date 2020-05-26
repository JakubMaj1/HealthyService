
    function ShowAll(mealId) {
        $(".meal" + mealId).toggleClass("hidden");

    }
    function sprawdz2() {

        var loadValue = parseFloat(document.getElementById("load").value);
        var repetitionValue = parseFloat(document.getElementById("repetition").value);
        var factor = 1.33 - (0.03 * (10 - repetitionValue));

        if (repetitionValue == 1)
            factor = 1;

        if (repetitionValue > 10) {
            var spanRepetition = $("#userMessageRepetition");
            spanRepetition.show();
            return;
        }
        else {
            var spanRepetition = $("#userMessageRepetition");
            spanRepetition.hide();
        }

        var Result2 = (loadValue * factor).toFixed(2);

        var Result250 = (Result2 * 0.5).toFixed(2);;

        var Result260 = (Result2 * 0.6).toFixed(2);;

        var Result270 = (Result2 * 0.7).toFixed(2);;

        var Result280 = (Result2 * 0.8).toFixed(2);;

        var Result290 = (Result2 * 0.9).toFixed(2);;

        if (loadValue > 0) {

            document.getElementById("MaxInfo").innerHTML = "Maksymalne obciążenie";
            document.getElementById("maxLoad").innerHTML = Result2 + " kg";
            document.getElementById("maxLoad50").innerHTML = "50 % ciężaru : " + (Result250) + " kg";
            document.getElementById("maxLoad60").innerHTML = "60 % ciężaru : " + (Result260) + " kg";
            document.getElementById("maxLoad70").innerHTML = "70 % ciężaru : " + (Result270) + " kg";
            document.getElementById("maxLoad80").innerHTML = "80 % ciężaru : " + (Result280) + " kg";
            document.getElementById("maxLoad90").innerHTML = "90 % ciężaru : " + (Result290) + " kg";
        }
    }

    function BMR() {
        var WeightBMRValue = parseFloat(document.getElementById("WeightBMR").value);
        var HeightBMRValue = parseFloat(document.getElementById("HeightBMR").value);
        var AgeBMRValue = parseFloat(document.getElementById("AgeBMR").value);
        var ActiveLevelBMRValue = parseFloat(document.getElementById("ActiveLevelBMR").value);
        var GenderBMRValue = (document.getElementById("GenderBMR").value);
        var Faktor = 1.9 - (0.2 * (4 - ActiveLevelBMRValue));
        var ResultBMR = (((9.99 * WeightBMRValue) + (6.25 * HeightBMRValue) - (4.92 * AgeBMRValue) + 5) * Faktor);
        ResultBMR = Math.round(ResultBMR);

        //  1.3         
        //  1.5
        //  1,7
        //  1.9




        if (HeightBMRValue > 100 && WeightBMRValue > 20 && HeightBMRValue < 240 && WeightBMRValue < 200) {

            switch (GenderBMRValue) {

                case "Mężczyzna":

                    document.getElementById("BMRValueInfo").innerHTML = "Aby utrzymać wagę ";
                    document.getElementById("BMRValue").innerHTML = ResultBMR + " kcal";
                    document.getElementById("BMRValueInfo2").innerHTML = "Aby stracić na wadze ";
                    document.getElementById("BMRValueMinus").innerHTML = (ResultBMR - 200) + " kcal";
                    document.getElementById("BMRValueInfo3").innerHTML = "Aby przybrać na wadze ";
                    document.getElementById("BMRValuePlus").innerHTML = (ResultBMR + 200) + " kcal";
                    break;

                case "Kobieta":
                    document.getElementById("BMRValueInfo").innerHTML = "Aby utrzymać wagę ";
                    document.getElementById("BMRValue").innerHTML = (ResultBMR - 166) + " kcal";
                    document.getElementById("BMRValueInfo2").innerHTML = "Aby stracić na wadze ";
                    document.getElementById("BMRValueMinus").innerHTML = ((ResultBMR - 166) - 150) + " kcal";
                    document.getElementById("BMRValueInfo3").innerHTML = "Aby przybrać na wadze ";
                    document.getElementById("BMRValuePlus").innerHTML = ((ResultBMR - 166) + 100) + " kcal";



                    break;

            }

        }
    }

    function BMI() {
        var WeightBMIvalue = parseFloat(document.getElementById("WeightBMI").value);
        var HeightBMIvalue = parseFloat(document.getElementById("HeightBMI").value);
        var ResultBMI = ((WeightBMIvalue / (HeightBMIvalue * HeightBMIvalue)) * 10000);
        ResultBMI = Math.round(ResultBMI * 100) / 100;

        if (HeightBMIvalue > 100 && WeightBMIvalue > 20 && HeightBMIvalue < 240 && WeightBMIvalue < 200) {
            document.getElementById("BMIInfo").innerHTML = "Wskaźnik BMI";
            document.getElementById("BMIValue").innerHTML = ResultBMI;
            document.getElementById("BMIInfo2").innerHTML = "Porównaj wynik z podanymi warościami";

        }

        else {
            document.getElementById("BMIInfo2").innerHTML = "Nieprawidłowa wartość";
        }


    }

    function Kcal(){
        var ProteinValue = document.getElementById("Protein").value;
        var CarboValue = document.getElementById("Carbo").value;
        var FatValue = document.getElementById("Fat").value;
        var ResultKcal = (ProteinValue * 4) + (CarboValue * 4) + (FatValue * 9);

        document.getElementById("KcalValue").innerHTML = ResultKcal;


    }

//    window.onload = BMI;

