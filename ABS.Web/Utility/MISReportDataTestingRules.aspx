<style>
    ul {
        list-style: none;
    }

        ul li::before {
            content: "\2022";
            color: blue;
            font-size: 30px;
            display: inline-block;
            width: 1em;
            margin-left: -1em;
        }
</style>
<body>

    <h1>MIS Report Data Testing Rules</h1>
    <h2>There are some common rules to follow report development, which is describe briefly as below.
    </h2>

    <ul>
        <li>All level of hierarchy (Distributor to National) – everywhere should be added it’s code beside name. Example: [REGION_NAME] - [REGION_CODE]. 
        </li>
        <li> There should be standard for decimal fraction and report standards like 
            <ul>
                <li>i. Metric Ton should be 2 decimals point,
                </li>
                <li>ii. Cartoon & Pcs figures for all report should be without fraction.
                </li>
                <li>iii. 1000 separation by comma for all data
                </li>
                <li>iv. If the value is 0, then it will be “- “,
                </li>
                <li>vi. Big Report should be in one page.  
                </li>
                <li>v. Brand sequence in all report,
                </li>
            </ul>
        </li>
        <li>Data should be sorted as per national code, division code, region code, zone code, distributor code. 
        </li>
        <li>Data synchronization between different reports. </li>
        <li>Should not view more data in report single page (first page), because it heavily slows down the loading process.
        </li>
        <li>Data testing as per national or without national selection.</li>
    </ul>







</body>
