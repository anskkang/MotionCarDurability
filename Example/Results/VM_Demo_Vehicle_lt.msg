 ! ANSYS WINDOWS SOLVER VERSION :: 8.0000e+00


=================================================================
< START OF ANSYS WINDOWS SOLVER ( ANSYS WINDOWS SOLVER VERSION = 8.00.30841 ) >
* DF_SOLVER::START TIME = Wed Jul 13 02:06:10 2022 PM
-----------------------------------------------------------------
* OS  = Windows 10 Enterprise
* CPU = 11th Gen Intel(R) Core(TM) i7-11850H @ 2.50GHz
* MEM = 64725 MB
-----------------------------------------------------------------

 < DF_SOLVER:: START READING A SSC FILE > 
 < DF_SOLVER:: END READING A SSC FILE > 


 < DF_SOLVER:: START READING A DFS FILE > 
INFORMATION::"/VM_Demo_Vehicle_lt/VMT_Steering/usub_steer_motion" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/VMT_Brake_System/usub_front_left_brake_torque" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/VMT_Brake_System/usub_front_right_brake_torque" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/VMT_Brake_System/usub_rear_left_brake_torque" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/VMT_Brake_System/usub_rear_right_brake_torque" function call flag is changed. (calling when report time only -> calling when every success time)
 ======================================================== 
 * DF_SOLVER:: TIRE ID = 0
 * TIRE TYPE = FTIRE
 *** COSIN FTire ::  
 *** COSIN FTire :: |----------------------------------------------------------------------|
 *** COSIN FTire :: |                                                                      |
 *** COSIN FTire :: |      @@@@@@ @@@@@@@@@@@@@@ @@@@                                      |
 *** COSIN FTire :: |   @@@@@@@@@ @@@@@@@@@@@@@@ @@@@                                      |
 *** COSIN FTire :: |  @@@@            @@@@      @@@@                                      |
 *** COSIN FTire :: |  @@@             @@@@      @@@@      cosin Tire and Road Interface   |
 *** COSIN FTire :: |  @@@@            @@@@      @@@@           connecting FTire and RGR   |
 *** COSIN FTire :: |   @@@@@@@@@@     @@@@      @@@@                             2021-4   |
 *** COSIN FTire :: |     @@@@@@@@@@   @@@@      @@@@                 r25682, 2021-10-25   |
 *** COSIN FTire :: |                                                                      |
 *** COSIN FTire :: |                                                                      |
 *** COSIN FTire :: |  installation directory:                               D:\OneDrive   |
 *** COSIN FTire :: |   - ANSYS, Inc\Win10_Data\        \cosin.2021-4-25682.x86_64\cosin   |
 *** COSIN FTire :: |  license agreement:        www.cosin.eu/contact/license-terms-eula   |
 *** COSIN FTire :: |  documentation and software updates:                  www.cosin.eu   |
 *** COSIN FTire :: |  in any support case, please read:       Help -> Support checklist   |
 *** COSIN FTire :: |                                                                      |
 *** COSIN FTire :: |  (c) 1997-2022       cosin scientific software AG, Munich, Germany   |
 *** COSIN FTire :: |                                                                      |
 *** COSIN FTire :: |----------------------------------------------------------------------|
 *** COSIN FTire ::  
 *** COSIN FTire :: | FTIRE CO-SIMULATION DRIVEN BY CTI
 *** COSIN FTire :: | started at July 13, 2022, 14:06, on platform win64
 *** COSIN FTire :: | cosin software will use status of version 2021-4 at July 13, 2022
 *** COSIN FTire :: |
 *** COSIN FTire :: | CALLING SOLVER
 *** COSIN FTire :: | Ansys Motion
 *** COSIN FTire :: |
 *** COSIN FTire :: | RESTRICTED USE OF COSIN LICENSE FEATURE ftire
 *** COSIN FTire :: | for 3rd-party interface development only.
 *** COSIN FTire :: | Must not be used for any other purposes!
 *** COSIN FTire :: |
 *** COSIN FTire :: | TIRE MODEL MESSAGES
 *** COSIN FTire :: | opening tire data file pass_car_235_45R17_94W.tir
 *** COSIN FTire :: | model type FTire, file format TeimOrbit
 *** COSIN FTire :: |
 *** COSIN FTire :: | FTIRE RUN-TIME CONFIGURATION
 *** COSIN FTire :: | - standard speed mode, set by run_time_mode in section [MODEL] of tir-file
 *** COSIN FTire :: |   note: this speed mode might be changed under CTI control at run-time
 *** COSIN FTire :: | - using latest FTire version available at day of execution (see above)
 *** COSIN FTire :: | BTW: you seem to be 2 versions behind the actual cosin software version; upgrade recommended
 *** COSIN FTire :: |
 *** COSIN FTire :: | data read in input file are regularly pre-processed
 *** COSIN FTire :: |
 *** COSIN FTire :: | MASS AND INERTIA CORRECTION IN MBS MODEL
 *** COSIN FTire :: | Either *add* the following mass properties to the *rim* in your suspension model ('rim-fixed'
 *** COSIN FTire :: | tire parts which are not accounted for in the tire model):
 *** COSIN FTire :: |   m =           4.410 kg
 *** COSIN FTire :: |   Ixx = Izz =   0.177 kgm^2
 *** COSIN FTire :: |   Iyy =         0.240 kgm^2  (y-axis = wheel spin axis)
 *** COSIN FTire :: |
 *** COSIN FTire :: | or *subtract* the following mass properties from the *wheel* (=rim+tire) in your suspension model
 *** COSIN FTire :: | ('free' tire parts which are accounted for in the tire model):
 *** COSIN FTire :: |   m =           8.190 kg
 *** COSIN FTire :: |   Ixx = Izz =   0.398 kgm^2
 *** COSIN FTire :: |   Iyy =         0.796 kgm^2  (y-axis = wheel spin axis)
 *** COSIN FTire :: |
 *** COSIN FTire :: | Estimated wheel moment of inertia about spin axis is 1.369 kgm^2.
 *** COSIN FTire :: |
 *** COSIN FTire :: | If this value differs from the respective value in your MBS model, adjust the rim moment of
 *** COSIN FTire :: | inertia in the tire data file accordingly. This is only necessary in real-time mode, to ensure
 *** COSIN FTire :: | numerical stability.
 *** COSIN FTire :: |
 *** COSIN FTire :: | FTIRE ANIMATION CONFIGURATION
 *** COSIN FTire :: | settings taken from tir-file
 *** COSIN FTire :: | animation from 0 s to 999 s with step size 1 ms
 *** COSIN FTire :: |
 *** COSIN FTire :: | ROAD MODEL MESSAGES
 *** COSIN FTire :: | inspecting road data file oval_belgian_block.rgr
 *** COSIN FTire :: | tire instance 1 uses road model type Regular Grid (rgr) with center-line
 *** COSIN FTire :: | rgr data file D:\OneDrive - ANSYS, Inc\Desktop\Standard Work_2\Roads\oval_belgian_block.rgr loaded
 *** COSIN FTire :: | nodes: 1001 x 372
 *** COSIN FTire :: | resolution: 10 x 10 mm
 *** COSIN FTire :: | compression ratio: 4.44 %
 *** COSIN FTire :: | bounding box rgr grid in curvi-linear coordinates: [0,10] x [-1.9,1.9] x [-23,120.8] m x m x mm
 *** COSIN FTire :: | file format: header ascii, data single precision binary little endian, z data, compressed
 *** COSIN FTire :: |
 *** COSIN FTire :: | loading RGR patch # 1: D:\OneDrive - ANSYS, Inc\Desktop\Standard
 *** COSIN FTire :: | Work_2\Roads\oval_belgian_block.rgr
 ======================================================== 
 * DF_SOLVER:: TIRE ID = 1
 * TIRE TYPE = FTIRE
 *** COSIN FTire :: |
 *** COSIN FTire :: | FTIRE RUN-TIME CONFIGURATION
 *** COSIN FTire :: | - standard speed mode, set by run_time_mode in section [MODEL] of tir-file
 *** COSIN FTire :: |   note: this speed mode might be changed under CTI control at run-time
 *** COSIN FTire :: | - using latest FTire version available at day of execution (see above)
 *** COSIN FTire :: | BTW: you seem to be 2 versions behind the actual cosin software version; upgrade recommended
 *** COSIN FTire :: |
 *** COSIN FTire :: | data read in input file are regularly pre-processed
 ======================================================== 
 * DF_SOLVER:: TIRE ID = 2
 * TIRE TYPE = FTIRE
 ======================================================== 
 * DF_SOLVER:: TIRE ID = 3
 * TIRE TYPE = FTIRE
INFORMATION::"/VM_Demo_Vehicle_lt/Usub_Steering_Demand" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Usub_Brake_Demand" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_powertrain_initial_condition" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Usub_Gear_Demand" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Usub_Throttle_Demand" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Usub_Clutch_Demand" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_throttle_angle" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_engine_torque" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_clutch_torque" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_transmission_torque" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_total_axle_torque" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_differential_torque" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_transmission_output_omega" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_transmission_input_omega" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_engine_acc" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Powertrain_lt/usub_clutch_slip" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Usub_Error_Longi" function call flag is changed. (calling when report time only -> calling when every success time)
INFORMATION::"/VM_Demo_Vehicle_lt/Usub_Error_Lat" function call flag is changed. (calling when report time only -> calling when every success time)
========================================================
 * GRAVITY :: 
0.0000e+00 0.0000e+00 -9.8066e+03
========================================================
 < DF_SOLVER:: END READING A DFS FILE > 


 < DF_SOLVER:: START READING A GINF FILE > 
 < DF_SOLVER:: END READING A GINF FILE > 


 < DF_SOLVER:: START CHECKING LICENSE > 
 < DF_SOLVER:: END CHECKING LICENSE > 


 < DF_SOLVER:: START PRE-ADDRESSING > 
========================================================
 * NO. OF PROCESSORS = 1
 * MY PROCESSOR = 0
 * NO. OF THREADS = 1
========================================================

 < DF_SOLVER:: PRE-ADDRESSING STEP 1 > 

 < DF_SOLVER:: PRE-ADDRESSING STEP 2 > 

 < DF_SOLVER:: PRE-ADDRESSING STEP 3 > 

 < DF_SOLVER:: PRE-ADDRESSING STEP 4 > 

 < DF_SOLVER:: PRE-ADDRESSING STEP 5 > 

 < DF_SOLVER:: PRE-ADDRESSING STEP 6 > 

 < DF_SOLVER:: PRE-ADDRESSING STEP 7 > 

 < DF_SOLVER:: PRE-ADDRESSING STEP 8 > 

 < DF_SOLVER:: PRE-ADDRESSING STEP 9 > 

 < DF_SOLVER:: PRE-ADDRESSING STEP 10 > 

  < DF_SOLVER:: END PRE-ADDRESSING > 


 < DF_SOLVER:: START ADDRESSING > 

 < DF_SOLVER:: ADDRESSING STEP 1 > 

 < DF_SOLVER:: ADDRESSING STEP 2 > 

 < DF_SOLVER:: ADDRESSING STEP 3 > 

 < DF_SOLVER:: ADDRESSING STEP 4 > 

 < DF_SOLVER:: ADDRESSING STEP 5 > 

 < DF_SOLVER:: ADDRESSING STEP 6 > 

 < DF_SOLVER:: ADDRESSING STEP 7 > 

 < DF_SOLVER:: ADDRESSING STEP 8 > 

 < DF_SOLVER:: ADDRESSING STEP 9 > 

 < DF_SOLVER:: ADDRESSING STEP 10 > 

 < DF_SOLVER:: ADDRESSING STEP 11 > 

 < DF_SOLVER:: ADDRESSING STEP 12 > 

 < DF_SOLVER:: ADDRESSING STEP 13 > 

 < DF_SOLVER:: ADDRESSING STEP 14 > 

-----------------------------------------------------------------
 < MASTER NODE ID AND POSITION >
-----------------------------------------------------------------

 < DF_SOLVER:: ADDRESSING STEP 15 > 

 < DF_SOLVER:: END ADDRESSING > 


 < DF_SOLVER:: START POST-ADDRESSING > 

 < DF_SOLVER:: END POST-ADDRESSING > 


-----------------------------------------------------------------
* DF_SOLVER:: WORKING DIRECTORY = D:\OneDrive - ANSYS, Inc\Desktop\Standard Work_2\Assemblies
-----------------------------------------------------------------

=================================================================
< START ANALYSIS >
* ANALYSIS START TIME = Wed Jul 13 02:06:17 2022 PM
-----------------------------------------------------------------

---------------------------------------------------------------
 DFSOLVER:: CO-SIMULATION MODE = 0
---------------------------------------------------------------

---------------------------------------------------------------

 SIMULATION PROGRESS = 0.0000e+00 % 

---------------------------------------------------------------

 < DF_SOLVER:: START POSITION ANALYSIS > 
 < THE NO. OF REDUNDANT CONSTRAINT = 0 > 

-----------------------------------------------------------------------------
 < POSITION ANALYSIS > GLOBAL ITERATION(FOR LARGE MOTION) = 1
-----------------------------------------------------------------------------

--------------------------------------------------
 < POSITION ANALYSIS > ITERATION = 0
 -------------------------------------------------- 
 * < POSITION ANALYSIS > MAXIMUM DELTA OF RBODY
 -------------------------------------------------- 
 * MAX. VALUE IN TRANSLATIONAL TERM = -1.0942e-06
 * BODY SEQUENTIAL ID = 24
 * COORDINATE = TRA. Z
 -------------------------------------------------- 
 * MAX. VALUE IN ROTATIONAL TERM = -1.4133e-08
 * BODY SEQUENTIAL ID = 18
 * COORDINATE = EULER. Z
 -------------------------------------------------- 

 -------------------------------------------------- 
 * < POSITION ANALYSIS > CONSTRAINT VIOLATION 
 -------------------------------------------------- 
 * MAX. VALUE IN TRANSLATIONAL TERM = -4.0319e-06
 * JOINT TYPE = BALL JOINT
 * JOINT SEQUENTIAL ID = 10
 * CONSTRAINT TYPE = TRA.X
 -------------------------------------------------- 
 * MAX. VALUE IN ROTATIONAL TERM = -5.8348e-10
 * JOINT TYPE = COUPLER
 * JOINT SEQUENTIAL ID = 1
 * CONSTRAINT TYPE = COUPLER ROT.
 -------------------------------------------------- 

--------------------------------------------------
 < POSITION ANALYSIS > NORM = 7.4583e-06
--------------------------------------------------

--------------------------------------------------
 < POSITION ANALYSIS > ITERATION = 1
 -------------------------------------------------- 
 * < POSITION ANALYSIS > MAXIMUM DELTA OF RBODY
 -------------------------------------------------- 
 * MAX. VALUE IN TRANSLATIONAL TERM = -2.6234e-09
 * BODY SEQUENTIAL ID = 8
 * COORDINATE = TRA. Y
 -------------------------------------------------- 
 * MAX. VALUE IN ROTATIONAL TERM = 1.3640e-08
 * BODY SEQUENTIAL ID = 18
 * COORDINATE = EULER. Y
 -------------------------------------------------- 

 -------------------------------------------------- 
 * < POSITION ANALYSIS > CONSTRAINT VIOLATION 
 -------------------------------------------------- 
 * MAX. VALUE IN TRANSLATIONAL TERM = 4.0874e-08
 * JOINT TYPE = MOTION
 * JOINT SEQUENTIAL ID = 1
 * CONSTRAINT TYPE = TRA.Z
 -------------------------------------------------- 
 * MAX. VALUE IN ROTATIONAL TERM = -7.1338e-09
 * JOINT TYPE = COUPLER
 * JOINT SEQUENTIAL ID = 0
 * CONSTRAINT TYPE = COUPLER ROT.
 -------------------------------------------------- 

--------------------------------------------------
 < POSITION ANALYSIS > NORM = 2.5358e-08
--------------------------------------------------
--------------------------------------------------
 < DF_SOLVER:: POSITION ANALYSIS SUCCESSFULLY FINISHED  > 
 < DF_SOLVER:: END POSITION ANALYSIS > 

---------------------------------------------------------------

 SIMULATION PROGRESS = 2.5000e+01 % 

---------------------------------------------------------------

 < DF_SOLVER:: START VELOCITY ANALYSIS > 
 < THE NO. OF VELOCITY CONSTRAINTS = 245 > 
 < THE NO. OF USER DEFINED INITIAL VELOCITIES = 53 > 
 < THE NO. OF TOTAL REDUNDANT CONSTRAINT = 11 > 
 < THE NO. OF REDUNDANT CONSTRAINT = 1 > 
 < THE NO. OF REDUNDANT INITIAL VELOCITY = 10 > 
 --------------------------------------------------------- 
 ..... 3RD TRANSLATIONAL JOINT MOTION.TRA
 ..... 7TH INITIAL VELOCITY FOR RIGID BODY TRANSLATION VELOCITY IN X
 ..... 24TH INITIAL VELOCITY FOR RIGID BODY TRANSLATION VELOCITY IN X
 ..... 26TH INITIAL VELOCITY FOR RIGID BODY TRANSLATION VELOCITY IN X
 ..... 27TH INITIAL VELOCITY FOR RIGID BODY TRANSLATION VELOCITY IN X
 ..... 35TH INITIAL VELOCITY FOR RIGID BODY TRANSLATION VELOCITY IN X
 ..... 37TH INITIAL VELOCITY FOR RIGID BODY TRANSLATION VELOCITY IN X
 ..... 38TH INITIAL VELOCITY FOR RIGID BODY TRANSLATION VELOCITY IN X
 ..... 44TH INITIAL VELOCITY FOR RIGID BODY TRANSLATION VELOCITY IN X
 ..... 45TH INITIAL VELOCITY FOR RIGID BODY TRANSLATION VELOCITY IN X
 ..... 46TH INITIAL VELOCITY FOR RIGID BODY TRANSLATION VELOCITY IN X
 --------------------------------------------------------- 
 < THE NO. OF INDEPENDENT COORDINATE = 103 > 
 --------------------------------------------------------- 
 ..... 1ST RIGID BODY TRANSLATION VELOCITY IN X
 ..... 2ND RIGID BODY TRANSLATION VELOCITY IN X
 ..... 3RD RIGID BODY TRANSLATION VELOCITY IN X
 ..... 4TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 5TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 6TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 8TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 9TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 10TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 11TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 12TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 13TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 14TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 15TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 16TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 17TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 18TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 19TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 20TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 21TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 22TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 23TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 25TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 28TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 29TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 30TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 31TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 32TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 33TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 34TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 36TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 39TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 40TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 41TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 42TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 42TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 43TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 43TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 44TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 45TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 47TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 48TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 49TH RIGID BODY TRANSLATION VELOCITY IN X
 ..... 21TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 34TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 10TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 1ST RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 2ND RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 40TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 19TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 4TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 20TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 9TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 6TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 41TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 20TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 21TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 3RD RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 1ST RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 30TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 29TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 31TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 11TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 11TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 19TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 4TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 19TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 31TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 9TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 11TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 31TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 20TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 29TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 44TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 12TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 4TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 12TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 28TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 30TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 21TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 10TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 15TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 10TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 34TH RIGID BODY ROTATIONAL  VELOCITY IN Y
 ..... 9TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 28TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 5TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 2ND RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 47TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 47TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 8TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 18TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 28TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 34TH RIGID BODY ROTATIONAL  VELOCITY IN X
 ..... 34TH RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 3RD RIGID BODY TRANSLATION VELOCITY IN Y
 ..... 12TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 30TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 34TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 18TH RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 23TH RIGID BODY TRANSLATION VELOCITY IN Z
 ..... 2ND RIGID BODY ROTATIONAL  VELOCITY IN Z
 ..... 3RD RIGID BODY TRANSLATION VELOCITY IN Z
 --------------------------------------------------------- 
 < DF_SOLVER:: VELOCITY ANALYSIS SUCCESSFULLY FINISHED  > 
 < DF_SOLVER:: END VELOCITY ANALYSIS > 

---------------------------------------------------------------

 SIMULATION PROGRESS = 5.0000e+01 % 

---------------------------------------------------------------

 < DF_SOLVER:: START ACCELERATION ANALYSIS > 
 < THE NO. OF REDUNDANT CONSTRAINT = 0 > 

 ========================================================= 
                    SYSTEM INFORMATION                     
 ========================================================= 
 < LINEAR SOLVER TYPE        = SPARSE SOLVER > 
 < JACOBIAN CALCULATE TYPE >
   - FINITE ELEMENT          = ANALYTIC
   - CONTACT (FLEX TO RIGID) = SIMPLIFY
   - CONTACT (FLEX TO FLEX)  = SIMPLIFY
 < STATIC ANALYSIS TYPE      = NON-LINEAR, WITH RIGID > 
 --------------------------------------------------------- 
 < SEARCHING CONTACT POINT = FREQUENTLY >
 < EVALUATION OF CONTACT ENTITY IN JACOBIAN = ALL >
 --------------------------------------------------------- 
 < THE DEGREE OF FREEDOM             = 104 > 
 < THE NO. OF VARIABLE EQUATION      = 14 > 
 < THE NO. OF 1ST DIFF. EQUATION     = 4 > 
   - IN TIRE MODULE                  = 0 > 
 < THE NO. OF 2ND DIFF. EQUATION     = 0 > 
 --------------------------------------------------------- 
 < THE NO. OF GENERALIZED COORDINATE = 300 > 
 < THE NO. OF CONSTRAINED EQUATION   = 196 > 
 --------------------------------------------------------- 
 < THE NO. OF RIGID BODY             = 50 > 
 < THE NO. OF FLEXIBLE BODY          = 0 > 
   - NODAL FE BODY                   = 0 > 
   - MODAL FE BODY                   = 0 > 
   - NODAL EasyFlex                  = 0 > 
   - MODAL EasyFlex                  = 0 > 
 < THE NO. OF NODE                   = 0 > 
   - FE NODE                         = 0 > 
   - OUTPUT NODE                     = 0 > 
 < THE NO. OF MODE                   = 0 > 
   - FE MODE                         = 0 > 
   - EasyFlex MODE                   = 0 > 
 < THE NO. OF EasyFlex NODE          = 0 > 
 --------------------------------------------------------- 
 < THE NO. OF FBEAM2                 = 0 > 
 < THE NO. OF FSHELL3                = 0 > 
   - FE FSHELL3                      = 0 > 
   - EasyFlex FSHELL3                = 0 > 
 < THE NO. OF FSHELL4                = 0 > 
 < THE NO. OF FSHELL6                = 0 > 
 < THE NO. OF FSHELL8                = 0 > 
 < THE NO. OF FSOLID4                = 0 > 
   - FE FSOLID4                      = 0 > 
   - EasyFlex FSOLID4                = 0 > 
 < THE NO. OF FSOLID5                = 0 > 
 < THE NO. OF FSOLID6                = 0 > 
 < THE NO. OF FSOLID8                = 0 > 
 < THE NO. OF FSOLID10               = 0 > 
 < THE NO. OF FSOLID13               = 0 > 
 < THE NO. OF FSOLID15               = 0 > 
 < THE NO. OF FSOLID20               = 0 > 
 --------------------------------------------------------- 
 < THE NO. OF MATERIAL               = 9 > 
 < THE NO. OF HYPER-ELASTIC MAT.     = 0 > 
 < THE NO. OF PLASTICITY MAT.        = 0 > 
 < THE NO. OF FOAM MAT.              = 0 > 
 --------------------------------------------------------- 
 < THE NO. OF R-T-R CONTACT          = 0 > 
 < THE NO. OF F-T-R CONTACT          = 0 > 
 < THE NO. OF F-T-F CONTACT          = 0 > 
 < THE NO. OF GENERAL CONTACT        = 0 > 
   - RIGID TO RIGID CASE             = 0 > 
   - FLEX TO RIGID CASE              = 0 > 
   - FLEX TO FLEX CASE               = 0 > 
 < THE NO. OF 2.5D CONTACT           = 0 > 
 < THE NO. OF TIE CONTACT            = 0 > 
 < THE NO. OF NTSF CONTACT           = 0 > 
 < THE NO. OF PTCV CONTACT           = 0 > 
 < THE NO. OF CVCV CONTACT           = 0 > 
 < THE NO. OF PTSF CONTACT           = 0 > 
 < THE NO. OF BC                     = 0 > 
 --------------------------------------------------------- 
 < THE NO. OF NURBS SURFACE          = 0 > 
 < THE NO. OF PIECEWISE SURFACE      = 0 > 
 < THE NO. OF PIECEWISE PATCH        = 0 > 
 < THE NO. OF PIECEWISE VERTEX       = 0 > 
 --------------------------------------------------------- 
 < THE NO. OF FE PLOAD               = 0 > 
 < THE NO. OF EasyFlex PLOAD         = 0 > 
 < THE NO. OF FE CLOAD               = 0 > 
 < THE NO. OF EasyFlex CLOAD         = 0 > 
 ========================================================= 

--------------------------------------------------
 < ACCELERATION ANALYSIS > ITERATION = 0
 *** COSIN FTire :: |
 *** COSIN FTire :: | ERROR
 *** COSIN FTire :: | variable rgr center-line friction not yet supported for modification
 *** COSIN FTire :: |
 *** COSIN FTire :: | BTW: you seem to be 2 versions behind the actual cosin software version;
 *** COSIN FTire :: | please consider to update in case of a support request related to the error message above
 *** COSIN FTire :: |
 *** COSIN FTire :: | potential instability of tire 2 at t = -0.0280s, x = 0.2325m, y = 0.7600m recognized when
 *** COSIN FTire :: | computing structural forces/moments
 *** COSIN FTire :: | potential instability of tire 2 at t = -0.0230s, x = 0.1908m, y = 0.7600m recognized when
 *** COSIN FTire :: | computing structural forces/moments
 *** COSIN FTire :: |
 -------------------------------------------------- 
 * < ACCELERATION ANALYSIS > MAXIMUM DELTA OF RBODY
 -------------------------------------------------- 
 * MAX. VALUE IN TRANSLATIONAL TERM = 5.3865e+06
 * BODY SEQUENTIAL ID = 4
 * COORDINATE = TRA. Z
 -------------------------------------------------- 
 * MAX. VALUE IN ROTATIONAL TERM = -1.4616e+04
 * BODY SEQUENTIAL ID = 36
 * COORDINATE = EULER. X
 -------------------------------------------------- 

 -------------------------------------------------- 
 * < ACCELERATION ANALYSIS > CONSTRAINT VIOLATION 
 -------------------------------------------------- 
 * MAX. VALUE IN TRANSLATIONAL TERM = -2.6752e-03
 * JOINT TYPE = CONVEL JOINT
 * JOINT SEQUENTIAL ID = 3
 * CONSTRAINT TYPE = TRA.X
 -------------------------------------------------- 
 * MAX. VALUE IN ROTATIONAL TERM = -3.7472e-05
 * JOINT TYPE = REVOLUTE JOINT
 * JOINT SEQUENTIAL ID = 3
 * CONSTRAINT TYPE = ROT.Y
 -------------------------------------------------- 

--------------------------------------------------
 < ACCELERATION ANALYSIS > NORM = 1.1051e+03
 < ACCELERATION ANALYSIS > CST NORM = 1.9585e+03
--------------------------------------------------

--------------------------------------------------
 < ACCELERATION ANALYSIS > ITERATION = 1
 -------------------------------------------------- 
 * < ACCELERATION ANALYSIS > MAXIMUM DELTA OF RBODY
 -------------------------------------------------- 
 * MAX. VALUE IN TRANSLATIONAL TERM = -3.4824e-10
 * BODY SEQUENTIAL ID = 3
 * COORDINATE = TRA. Z
 -------------------------------------------------- 
 * MAX. VALUE IN ROTATIONAL TERM = -2.8165e-12
 * BODY SEQUENTIAL ID = 36
 * COORDINATE = EULER. X
 -------------------------------------------------- 

 -------------------------------------------------- 
 * < ACCELERATION ANALYSIS > CONSTRAINT VIOLATION 
 -------------------------------------------------- 
 * MAX. VALUE IN TRANSLATIONAL TERM = 1.1820e+05
 * JOINT TYPE = MOTION
 * JOINT SEQUENTIAL ID = 1
 * CONSTRAINT TYPE = TRA.Z
 -------------------------------------------------- 
 * MAX. VALUE IN ROTATIONAL TERM = -2.0629e+04
 * JOINT TYPE = COUPLER
 * JOINT SEQUENTIAL ID = 0
 * CONSTRAINT TYPE = COUPLER ROT.
 -------------------------------------------------- 

--------------------------------------------------
 < ACCELERATION ANALYSIS > NORM = 2.9837e-12
 < ACCELERATION ANALYSIS > CST NORM = 1.1003e-10
--------------------------------------------------
--------------------------------------------------
 < DF_SOLVER:: ACCELERATION ANALYSIS SUCCESSFULLY FINISHED  > 
 < DF_SOLVER:: END ACCELERATION ANALYSIS > 

---------------------------------------------------------------

 SIMULATION PROGRESS = 7.5000e+01 % 

---------------------------------------------------------------

 < DF_SOLVER:: START DYNAMICS ANALYSIS > 
 < NUMERICAL DAMPING = 1.0000e+00 > 
 ---------------------------------------------------------------------------------------
    TIME   :: STEPSIZE ::   ANORM  :: NRES :: NJAC :: STEP :: NFAIL :: IFAIL :: IPCG 
 ---------------------------------------------------------------------------------------
 0.0000e+00 1.0000e-05 0.0000e+00      9      8      0      0      0      0
 1.0000e-05 1.0000e-05 3.2804e-05     12      9      1      0      0      0
 * 3.2463e-05   * 3.4036e-07  
 3.0000e-05 2.0000e-05 7.5844e-05     15     10      2      0      0      0
 * 7.5143e-05   * 7.0068e-07  
 1.5000e-04 4.0000e-05 7.9641e-05     24     13      5      0      0      0
 * 7.9384e-05   * 2.5660e-07  
 9.5000e-04 8.0000e-05 8.0843e-05     54     23     15      0      0      0
 * 8.0837e-05   * 6.1686e-09  
 2.7100e-03 1.6000e-04 5.6386e-04     87     34     26      0      0      0
 * 5.6378e-04   * 8.0755e-08  
 6.2300e-03 3.2000e-04 3.1669e-03    120     45     37      0      0      0
 * 3.1665e-03   * 3.8987e-07  
 1.3270e-02 6.4000e-04 7.7618e-03    153     56     48      0      0      0
 * 7.7582e-03   * 3.5615e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 2.0090e+00 % 
-------------------------------------------------------------------------------
 6.0270e-02 1.0000e-03 5.9558e-03    294    103     95      0      0      0
 * 5.9488e-03   * 6.9839e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 4.0090e+00 % 
-------------------------------------------------------------------------------
 1.2027e-01 1.0000e-03 5.5551e-03    474    163    155      0      0      0
 * 5.5493e-03   * 5.7641e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 6.0090e+00 % 
-------------------------------------------------------------------------------
 1.8027e-01 1.0000e-03 1.4421e-03    654    223    215      0      0      0
 * 1.4382e-03   * 3.8548e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 8.0090e+00 % 
-------------------------------------------------------------------------------
 2.4027e-01 1.0000e-03 1.4284e-03    834    283    275      0      0      0
 * 1.4258e-03   * 2.6819e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 1.0009e+01 % 
-------------------------------------------------------------------------------
 3.0027e-01 1.0000e-03 2.9407e-03   1014    343    335      0      0      0
 * 2.9380e-03   * 2.6888e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 1.2009e+01 % 
-------------------------------------------------------------------------------
 3.6027e-01 1.0000e-03 2.5118e-03   1194    403    395      0      0      0
 * 2.4940e-03   * 1.7805e-05  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 1.4009e+01 % 
-------------------------------------------------------------------------------
 4.2027e-01 1.0000e-03 2.1013e-03   1374    463    455      0      0      0
 * 2.0991e-03   * 2.2455e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 1.6009e+01 % 
-------------------------------------------------------------------------------
 4.8027e-01 1.0000e-03 1.2693e-03   1554    523    515      0      0      0
 * 1.2680e-03   * 1.2819e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 1.8009e+01 % 
-------------------------------------------------------------------------------
 5.4027e-01 1.0000e-03 2.3000e-03   1734    583    575      0      0      0
 * 2.2946e-03   * 5.4651e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 2.0009e+01 % 
-------------------------------------------------------------------------------
 6.0027e-01 1.0000e-03 2.2539e-03   1914    643    635      0      0      0
 * 2.2515e-03   * 2.3628e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 2.2009e+01 % 
-------------------------------------------------------------------------------
 6.6027e-01 1.0000e-03 3.5308e-03   2094    703    695      0      0      0
 * 3.5248e-03   * 5.9279e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 2.4009e+01 % 
-------------------------------------------------------------------------------
 7.2027e-01 1.0000e-03 2.9705e-03   2274    763    755      0      0      0
 * 2.9659e-03   * 4.5761e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 2.6009e+01 % 
-------------------------------------------------------------------------------
 7.8027e-01 1.0000e-03 2.0652e-03   2454    823    815      0      0      0
 * 2.0611e-03   * 4.1560e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 2.8009e+01 % 
-------------------------------------------------------------------------------
 8.4027e-01 1.0000e-03 1.7085e-03   2634    883    875      0      0      0
 * 1.7005e-03   * 8.0487e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 3.0009e+01 % 
-------------------------------------------------------------------------------
 9.0027e-01 1.0000e-03 3.0444e-03   2814    943    935      0      0      0
 * 3.0404e-03   * 4.0344e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 3.2009e+01 % 
-------------------------------------------------------------------------------
 9.6027e-01 1.0000e-03 1.8882e-03   2994   1003    995      0      0      0
 * 1.8857e-03   * 2.5166e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 3.4009e+01 % 
-------------------------------------------------------------------------------
 1.0203e+00 1.0000e-03 1.4426e-03   3174   1063   1055      0      0      0
 * 1.4397e-03   * 2.9368e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 3.6009e+01 % 
-------------------------------------------------------------------------------
 1.0803e+00 1.0000e-03 1.8704e-03   3354   1123   1115      0      0      0
 * 1.8686e-03   * 1.7849e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 3.8009e+01 % 
-------------------------------------------------------------------------------
 1.1403e+00 1.0000e-03 2.4279e-03   3534   1183   1175      0      0      0
 * 2.4247e-03   * 3.2489e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 4.0009e+01 % 
-------------------------------------------------------------------------------
 1.2003e+00 1.0000e-03 2.4843e-03   3714   1243   1235      0      0      0
 * 2.4775e-03   * 6.8607e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 4.2009e+01 % 
-------------------------------------------------------------------------------
 1.2603e+00 1.0000e-03 2.0739e-03   3894   1303   1295      0      0      0
 * 2.0716e-03   * 2.2880e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 4.4009e+01 % 
-------------------------------------------------------------------------------
 1.3203e+00 1.0000e-03 1.9372e-03   4074   1363   1355      0      0      0
 * 1.9345e-03   * 2.6850e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 4.6009e+01 % 
-------------------------------------------------------------------------------
 1.3803e+00 1.0000e-03 2.1425e-03   4254   1423   1415      0      0      0
 * 2.1385e-03   * 4.0558e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 4.8009e+01 % 
-------------------------------------------------------------------------------
 1.4403e+00 1.0000e-03 1.8154e-03   4434   1483   1475      0      0      0
 * 1.8107e-03   * 4.6280e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 5.0009e+01 % 
-------------------------------------------------------------------------------
 1.5003e+00 1.0000e-03 2.0563e-03   4614   1543   1535      0      0      0
 * 2.0527e-03   * 3.5906e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 5.2009e+01 % 
-------------------------------------------------------------------------------
 1.5603e+00 1.0000e-03 2.2365e-03   4794   1603   1595      0      0      0
 * 2.2326e-03   * 3.9312e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 5.4009e+01 % 
-------------------------------------------------------------------------------
 1.6203e+00 1.0000e-03 2.3154e-03   4974   1663   1655      0      0      0
 * 2.3090e-03   * 6.4259e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 5.6009e+01 % 
-------------------------------------------------------------------------------
 1.6803e+00 1.0000e-03 1.9865e-03   5154   1723   1715      0      0      0
 * 1.9816e-03   * 4.8413e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 5.8009e+01 % 
-------------------------------------------------------------------------------
 1.7403e+00 1.0000e-03 1.9048e-03   5334   1783   1775      0      0      0
 * 1.9017e-03   * 3.0645e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 6.0009e+01 % 
-------------------------------------------------------------------------------
 1.8003e+00 1.0000e-03 1.5925e-03   5514   1843   1835      0      0      0
 * 1.5903e-03   * 2.2684e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 6.2009e+01 % 
-------------------------------------------------------------------------------
 1.8603e+00 1.0000e-03 2.0881e-03   5694   1903   1895      0      0      0
 * 2.0857e-03   * 2.3845e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 6.4009e+01 % 
-------------------------------------------------------------------------------
 1.9203e+00 1.0000e-03 2.4757e-03   5874   1963   1955      0      0      0
 * 2.4429e-03   * 3.2799e-05  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 6.6009e+01 % 
-------------------------------------------------------------------------------
 1.9803e+00 1.0000e-03 1.7523e-03   6054   2023   2015      0      0      0
 * 1.7499e-03   * 2.3965e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 6.8009e+01 % 
-------------------------------------------------------------------------------
 2.0403e+00 1.0000e-03 2.3543e-03   6234   2083   2075      0      0      0
 * 2.3516e-03   * 2.7299e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 7.0009e+01 % 
-------------------------------------------------------------------------------
 2.1003e+00 1.0000e-03 1.1402e-03   6414   2143   2135      0      0      0
 * 1.1386e-03   * 1.6068e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 7.2009e+01 % 
-------------------------------------------------------------------------------
 2.1603e+00 1.0000e-03 3.0682e-03   6594   2203   2195      0      0      0
 * 3.0653e-03   * 2.8111e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 7.4009e+01 % 
-------------------------------------------------------------------------------
 2.2203e+00 1.0000e-03 2.2509e-03   6774   2263   2255      0      0      0
 * 2.2475e-03   * 3.3836e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 7.6009e+01 % 
-------------------------------------------------------------------------------
 2.2803e+00 1.0000e-03 1.5423e-03   6954   2323   2315      0      0      0
 * 1.5398e-03   * 2.5001e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 7.8009e+01 % 
-------------------------------------------------------------------------------
 2.3403e+00 1.0000e-03 1.5095e-03   7134   2383   2375      0      0      0
 * 1.5075e-03   * 2.0905e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 8.0009e+01 % 
-------------------------------------------------------------------------------
 2.4003e+00 1.0000e-03 2.1674e-03   7314   2443   2435      0      0      0
 * 2.1646e-03   * 2.8006e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 8.2009e+01 % 
-------------------------------------------------------------------------------
 2.4603e+00 1.0000e-03 1.1367e-03   7494   2503   2495      0      0      0
 * 1.1357e-03   * 9.7990e-07  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 8.4009e+01 % 
-------------------------------------------------------------------------------
 2.5203e+00 1.0000e-03 3.0033e-03   7674   2563   2555      0      0      0
 * 2.9959e-03   * 7.4333e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 8.6009e+01 % 
-------------------------------------------------------------------------------
 2.5803e+00 1.0000e-03 2.2914e-03   7854   2623   2615      0      0      0
 * 2.2893e-03   * 2.1294e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 8.8009e+01 % 
-------------------------------------------------------------------------------
 2.6403e+00 1.0000e-03 2.5578e-03   8034   2683   2675      0      0      0
 * 2.5555e-03   * 2.2734e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 9.0009e+01 % 
-------------------------------------------------------------------------------
 2.7003e+00 1.0000e-03 2.5777e-03   8214   2743   2735      0      0      0
 * 2.5740e-03   * 3.7051e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 9.2009e+01 % 
-------------------------------------------------------------------------------
 2.7603e+00 1.0000e-03 1.9657e-03   8394   2803   2795      0      0      0
 * 1.9553e-03   * 1.0350e-05  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 9.4009e+01 % 
-------------------------------------------------------------------------------
 2.8203e+00 1.0000e-03 2.7111e-03   8574   2863   2855      0      0      0
 * 2.7070e-03   * 4.1041e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 9.6009e+01 % 
-------------------------------------------------------------------------------
 2.8803e+00 1.0000e-03 4.4946e-03   8754   2923   2915      0      0      0
 * 4.4585e-03   * 3.6060e-05  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 9.8009e+01 % 
-------------------------------------------------------------------------------
 2.9403e+00 1.0000e-03 1.9607e-03   8934   2983   2975      0      0      0
 * 1.9581e-03   * 2.6070e-06  
 2.9983e+00 1.0000e-03 2.3234e-03   9108   3041   3033      0      0      0
 * 2.3197e-03   * 3.6761e-06  
-------------------------------------------------------------------------------
 ANALYSIS PROGRESS = 1.0000e+02 % 
-------------------------------------------------------------------------------
 3.0000e+00 1.7300e-03 1.3770e-02   9111   3042   3034      0      0      0
 * 1.3754e-02   * 1.5310e-05  

 < DF_SOLVER:: END DYNAMICS ANALYSIS > 

---------------------------------------------------------------

 SIMULATION PROGRESS = 1.0000e+02 % 

---------------------------------------------------------------

-----------------------------------------------------------------
 < DF_SOLVER:: FUNCTION EVALUATION RESULT >
-----------------------------------------------------------------
\                           MEMORY(MB)      TIME(SEC)      COUNT
 TOTAL                          679.33         243.04          1

         * PRE PROCESSING *
 READ_DFSFILE                   415.00           0.95          1
 READ_GINFFILE                    0.00           0.00          1
 PRE_ADDRESSING                   2.32           0.00          1
 ADDRESSING                       9.99           0.00          1
 POST_ADDRESSING                  0.00           0.00          1

         * ANALYSIS *
 POSITION_ANALYSIS                9.98           0.11          1
 VELOCITY_ANALYSIS                0.00           0.02          1
 ACCELERATION_ANALYSIS            4.71           5.91          1
 STATIC_ANALYSIS                  0.00           0.00          0
 DYNAMICS_ANALYSIS              221.29         230.64          1
 EIGENVALUE_ANALYSIS              0.00           0.00          0
 THERMAL_ANALYSIS                 0.00           0.00          0

         * SPARSE LINEAR SOLVER *
 SPS_STEP_1                       0.00           0.00          0
 SPS_STEP_2                       0.00           0.00          0
 SPS_STEP_3                       0.00           0.00          0
 SPS_STEP_4                       0.00           0.00          0
 SPS_STEP_5                       0.00           0.00          0
 SPS_STEP_6                       1.45           0.11          3
 SPS_STEP_7                       8.31           1.24       3038
 SPS_STEP_8                       0.16           0.83       6072
 SPS_STEP_9                       2.64           1.53       6072

         * ITERATION LINEAR SOLVER *
 ITE_STEP_1                       0.00           0.00          0
 ITE_STEP_2                       0.00           0.00          0
 ITE_STEP_3                       0.00           0.00          0
 ITE_STEP_4                       0.00           0.00          0
 ITE_STEP_5                       0.00           0.00          0

         * FRONTAL & SUPER SOLVER *
 FRT_STEP_1                       0.00           0.00          0
 FRT_STEP_2                       0.00           0.00          0
 FRT_STEP_3                       0.00           0.00          0
 FRT_STEP_4                       0.00           0.00          0
 FRT_STEP_5                       0.00           0.00          0
 FRT_STEP_6                       0.00           0.00          0

         * GENERAL PROCESS *
 JACRES                           0.00           6.07       3038
 RECOVER_1                        0.00           0.09       9105
 RECOVER_2                      121.79         232.13       9105
 RECOVER_3                        0.00           0.01       3035
 RECOVER_4                        0.00           0.00          0

         * EIGEN PROCESS *
 EIG_PROCESS                      0.00           0.00          0
 GATHERING_EIGEN                  0.00           0.00          0

         * CONTACT *
 CONTACT                          0.00           0.00          0
 - UPDATE_INFORMATION             0.00           0.00          0
 - FIND_POINT                     0.00           0.00          0
   - GENERAL CONTACT              0.00           0.00          0
   - 3D CONTACT                   0.00           0.00          0
   - 25D CONTACT                  0.00           0.00          0
 - PREGET_RESJAC                  0.00           0.00          0

         * REPORT *
 REPORT                           0.00           0.17       3035
 -----------------------------------------------------------------
 < USED TIME = 0 H/ 3 M/ 56 S ( 236.68 SECONDS ) >
 -----------------------------------------------------------------

-----------------------------------------------------------------
* DF_SOLVER::END TIME   = Wed Jul 13 02:10:13 2022 PM
< END OF ANSYS WINDOWS SOLVER >
=================================================================
 *** COSIN FTire :: |
 *** COSIN FTire :: | CTI AND FTIRE STATISTICS
 *** COSIN FTire :: | statistics tire 1
 *** COSIN FTire :: | number of tire initializations: 1
 *** COSIN FTire :: | number of steps: 3199
 *** COSIN FTire :: | number of repeated steps: 9
 *** COSIN FTire :: | number of state array saves: 0
 *** COSIN FTire :: | number of state array retrieves: 0
 *** COSIN FTire :: | number of resets: 0
 *** COSIN FTire :: | no recognized issues with input signals
 *** COSIN FTire :: |
 *** COSIN FTire :: | statistics tire 2
 *** COSIN FTire :: | number of tire initializations: 3
 *** COSIN FTire :: | number of steps: 3354
 *** COSIN FTire :: | number of repeated steps: 9
 *** COSIN FTire :: | number of state array saves: 0
 *** COSIN FTire :: | number of state array retrieves: 0
 *** COSIN FTire :: | number of resets: 0
 *** COSIN FTire :: | no recognized issues with input signals
 *** COSIN FTire :: |
 *** COSIN FTire :: | statistics tire 3
 *** COSIN FTire :: | number of tire initializations: 1
 *** COSIN FTire :: | number of steps: 3178
 *** COSIN FTire :: | number of repeated steps: 9
 *** COSIN FTire :: | number of state array saves: 0
 *** COSIN FTire :: | number of state array retrieves: 0
 *** COSIN FTire :: | number of resets: 0
 *** COSIN FTire :: | no recognized issues with input signals
 *** COSIN FTire :: |
 *** COSIN FTire :: | statistics tire 4
 *** COSIN FTire :: | number of tire initializations: 1
 *** COSIN FTire :: | number of steps: 3134
 *** COSIN FTire :: | number of repeated steps: 9
 *** COSIN FTire :: | number of state array saves: 0
 *** COSIN FTire :: | number of state array retrieves: 0
 *** COSIN FTire :: | number of resets: 0
 *** COSIN FTire :: | no recognized issues with input signals
 *** COSIN FTire :: |
 *** COSIN FTire :: | last speed mode of all tires: 0
 *** COSIN FTire :: | total elapsed time: 4m 3s
 *** COSIN FTire :: | real-time factor vehicle model + FTire: 77.009
 *** COSIN FTire :: | number of road evaluations: 367.4 mio (exactly: 367375511)
 *** COSIN FTire :: | 2 warning messages:
 *** COSIN FTire :: | potential instability of tire 2 at t = -0.0280s, x = 0.2325m, y = 0.7600m recognized when
 *** COSIN FTire :: | computing structural forces/moments
 *** COSIN FTire :: | potential instability of tire 2 at t = -0.0230s, x = 0.1908m, y = 0.7600m recognized when
 *** COSIN FTire :: | computing structural forces/moments
 *** COSIN FTire :: | 4 error messages:
 *** COSIN FTire :: | variable rgr center-line friction not yet supported for modification
 *** COSIN FTire :: | (several times)
 *** COSIN FTire :: |
 *** COSIN FTire :: | FILES AND RESOURCES
 *** COSIN FTire :: | (i)=input file  (o)=output file  (r)=resource file
 *** COSIN FTire :: | (l)=library  (e)=executable  (f)=folder  (n)=option
 *** COSIN FTire :: |
 *** COSIN FTire :: | in cosin installation directory
 *** COSIN FTire :: | D:\OneDrive - ANSYS, Inc\Win10_Data\        \cosin.2021-4-25682.x86_64\cosin
 *** COSIN FTire :: | (l) cosin library:              lib\win64\cti.dll
 *** COSIN FTire :: | (r) graphics resources:         share\cosingl.mat
 *** COSIN FTire :: |
 *** COSIN FTire :: | in output folder
 *** COSIN FTire :: | D:\OneDrive - ANSYS, Inc\Desktop\Standard Work_2\Assemblies
 *** COSIN FTire :: | (i) texture image in animation: VM_Demo_Vehicle_lt_ftire004.png
 *** COSIN FTire :: | (o) diagnosis file:             VM_Demo_Vehicle_lt_ftire_diagnosis.txt
 *** COSIN FTire :: | (o) log file:                   VM_Demo_Vehicle_lt_ftire.log
 *** COSIN FTire :: | (o) png file:                   VM_Demo_Vehicle_lt_ftire001.png
 *** COSIN FTire :: | (o) png file:                   VM_Demo_Vehicle_lt_ftire002.png
 *** COSIN FTire :: | (o) png file:                   VM_Demo_Vehicle_lt_ftire003.png
 *** COSIN FTire :: | (o) png file:                   VM_Demo_Vehicle_lt_ftire004.png
 *** COSIN FTire :: |
 *** COSIN FTire :: | in home directory
 *** COSIN FTire :: | C:\Users\hjun
 *** COSIN FTire :: | (r) cosin ini file:             cosin.ini
 *** COSIN FTire :: |
 *** COSIN FTire :: | in other directories (or resource other than file)
 *** COSIN FTire :: | (e) calling solver:             C:\Program Files\ANSYS Inc\v221\Motion\Solver\Windows\rundfs.exe
 *** COSIN FTire :: | (i) FTire data file:            D:\OneDrive - ANSYS, Inc\Desktop\Standard Work_2\Tires\pass_car ..
 *** COSIN FTire :: |                                 _235_45R17_94W.tir
 *** COSIN FTire :: | (i) road data file:             D:\OneDrive - ANSYS, Inc\Desktop\Standard Work_2\Roads\oval_bel ..
 *** COSIN FTire :: |                                 gian_block.rgr
 *** COSIN FTire :: | (n) cosin license server:       seow19molic01:80 (defined in cosin ini file)
 *** COSIN FTire :: |
 *** COSIN FTire :: | all cosin licenses returned, if used any. Bye.
 *** COSIN FTire :: |
