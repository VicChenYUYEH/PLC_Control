using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace HyTemplate
{ 
    class MelsecAccessor
    {
        private ActUtlTypeLib.ActUtlType lpcom_ReferencesUtlType;
        int iLogicalStationNumber = 3;      //LogicalStationNumber for ActUtlType

        public MelsecAccessor(int m_LogicalStationNumber)
        {
            lpcom_ReferencesUtlType = new ActUtlTypeLib.ActUtlType();
            iLogicalStationNumber = m_LogicalStationNumber;
        }
        
        public int Open()
        {
            int iReturnCode;				//Return code            

           //Set the value of 'LogicalStationNumber' to the property.
            lpcom_ReferencesUtlType.ActLogicalStationNumber = iLogicalStationNumber;

            //Set the value of 'Password'.
            //lpcom_ReferencesUtlType.ActPassword = txt_Password.Text;

            //The Open method is executed.
            iReturnCode = lpcom_ReferencesUtlType.Open();

            return iReturnCode;
        }

        public int Close()
        {
            int iReturnCode;
            iReturnCode = lpcom_ReferencesUtlType.Close();

            return iReturnCode;
        }

        public int readDeviceBlock(string m_Device, int m_Size, out short[] m_Values)
        {
            int iReturnCode = -1;			    //Return code
            String szDeviceName = m_Device;		//List data for 'DeviceName'
            int iNumberOfData = m_Size;			//Data for 'DeviceSize'
            short[] arrDeviceValue;		        //Data for 'DeviceValue'
            int iNumber;					    //Loop counter

            //m_Values = new System.String[0];

            //Assign the array for 'DeviceValue'.
            m_Values/*arrDeviceValue*/ = new short[iNumberOfData];

            //
            //Processing of ReadDeviceBlock2 method
            //
            try
            {
                //The ReadDeviceBlock2 method is executed.
                iReturnCode = lpcom_ReferencesUtlType.ReadDeviceBlock2(szDeviceName,
                                                             iNumberOfData,
                                                             out m_Values[0]/*arrDeviceValue*/);
            }
            //Exception processing			
            catch (Exception exception)
            {
                //MessageBox.Show(exception.Message, Name,
                //                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return iReturnCode;
            }

            return iReturnCode;
        }

        
        public int writeDeviceRandom2(string m_Device, short m_Value)
        {
            int iReturnCode = -1;		    //Return code
            String szDeviceName = m_Device;	//List data for 'DeviceName'
            int iNumberOfData = 1;			//Data for 'DeviceSize'
            short[] arrDeviceValue;		    //Data for 'DeviceValue'
            int iNumber;					//Loop counter
            int iSizeOfIntArray;		    //
                                   
            //Get size for 'DeviceValue'
            iSizeOfIntArray = 1;
            //Assign the array for 'DeviceValue'.
            arrDeviceValue = new short[iNumberOfData];

            //Convert the 'DeviceValue'.
            for (iNumber = 0; iNumber < iSizeOfIntArray; iNumber++)
            {
                try
                {
                    arrDeviceValue[iNumber] = m_Value;// Convert.ToInt16(m_Value);
                }

                //Exception processing
                catch (Exception exExcepion)
                {
                    //MessageBox.Show(exExcepion.Message,
                    //                  Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return iReturnCode ;
                }
            }

            //
            //Processing of WriteDeviceRandom2 method
            //
            try
            {
                //The WriteDeviceRandom2 method is executed.
                iReturnCode = lpcom_ReferencesUtlType.WriteDeviceRandom2(szDeviceName,
                                                                iNumberOfData,
                                                                ref arrDeviceValue[0]);
            }
            //Exception processing			
            catch (Exception exception)
            {
                //MessageBox.Show(exception.Message, Name,
                //                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return iReturnCode;
            }

            //The return code of the method is displayed by the hexadecimal.
            return iReturnCode;
        }
    }
}
